import * as THREE from 'three';

import { MTLLoader } from './libs/MTLLoader.js';
import { GLTFLoader } from './libs/GLTFLoader.js';
import { DDSLoader } from './libs/DDSLoader.js';
import { OBJLoader } from './libs/OBJLoader.js';
import { STLLoader } from './libs/STLLoader.js';

var container, camera, scene, renderer;
// создание загрузчика текстур
var loader = new THREE.TextureLoader();
var clock = new THREE.Clock();
// загрузка текстуры grasstile.jpg из папки pics
var tex = loader.load('./pics/grasstile.jpg');
var N = 255;
var geometry;

// глобальные переменные для хранения списка анимаций
var mixer;
var morphs = [];

var cursor, circle;
var radius = 10;
var bD = 0;

// экранные координаты курсора мыши
var mouse = { x: 0, y: 0 }; 
// массив для объектов, проверяемых на пересечение с курсором
var targetList = [];    

// объект интерфейса и его ширина  
var gui = new dat.GUI();
gui.width = 200;
var brVal = false; //стоит ли галочка

var rotationSX;
var rotationSY;
var rotationSZ;

var scaleSX;
var scaleSY;
var scaleSZ;

var loadModels = [
    ['./models/house/', 'Cyprys_House.obj', 'Cyprys_House.mtl', 10, 'hous', false],
    ['./models/fence/', 'grade.obj', 'grade.mtl', 5, 'grade', false],
    ['../models/bushes/fern/', '10443_Fern_v2_max2011_it2.obj', '10443_Fern_v2_max2011_it2.mtl', 0.3, 'fern', true],
    ['../models/trees/cactus/', '10436_Cactus_v1_max2010_it2.obj', '10436_Cactus_v1_max2010_it2.mtl', 0.2, 'cactus', true],
    ['../models/trees/pine/', '10447_Pine_Tree_v1_L3b.obj', '10447_Pine_Tree_v1_L3b.mtl', 0.1, 'pine', true]
];

var loadAnimModels = [
    ['./models/animals/Parrot.glb', 0.2, 'parrot'],
    ['./models/animals/Flamingo.glb', 0.2, 'flamingo']
];

var guiModelsAdd = [
    ['addHouse', 'add house'],
    ['addGrade', 'add grade'],
    ['addFern', 'add fern'],
    ['addCactus', 'add cactus'],
    ['addPine', 'add pine'],
    ['addParrot', 'add parrot'],
    ['addFlamingo', 'add flamingo']
];

var models = new Map;
var modelsOnScene = [];
var mixersOnScene = [];

var selected, press;
var ID = 0;

init();

animate();

//считает сколько раз процент загрузки был равен 100, чтобы вывести alert
var count = 0;
//ниже всё по загрузке и удалению моделей (часть 1)
function loadModel(path, objName, mtlName, s, name, rotation)
{
    // функция, выполняемая в процессе загрузки модели (выводит процент загрузки)
    var onProgress = function(xhr) {
        if (xhr.lengthComputable) {
            var percentComplete = Math.round(xhr.loaded / xhr.total * 100, 2); //xhr.loaded / xhr.total * 100;
            console.log(percentComplete + '% downloaded' );
            if (percentComplete == 100) count++;
            if (count == loadModels.length) al();
        }
    };
    // функция, выполняющая обработку ошибок, возникших в процессе загрузки
    var onError = function(xhr) { };

    const manager = new THREE.LoadingManager();

    new MTLLoader( manager )
        .setPath( path )
        .load ( mtlName, function( materials ){
            new OBJLoader( manager )
            .setMaterials( materials )
            .setPath( path )
            .load( objName, function( object ){
                if (rotation == true)
                object.rotation.x = - Math.PI / 2;
                object.scale.set( s, s, s);

                object.traverse(function( child ){
                    if ( child instanceof THREE.Mesh){
                        child.castShadow = true;
                        child.receiveShadow = true;
                        child.parent = object;
                    }
                });

                object.name = name;

                models.set(name, object);
                //scene.add( object );
        }, onProgress, onError);
    });
}

function loadAnimatedModel(path, s)
{
    var loader = new GLTFLoader();
    loader.load(path, function (gltf) {
        var mesh = gltf.scene.children[0];
        var clip = gltf.animations[0];
        mixer = new THREE.AnimationMixer(mesh);

        // установка параметров анимации (скорость воспроизведения и стартовый кадр)
        mixer.clipAction(clip, mesh).setDuration(1).startAt(0).play();

        console.log(gltf.animations);

        mesh.scale.set(s, s, s);

        mesh.position.y = getRandomNumber(80, 100);
        mesh.position.x = getRandomNumber(0, N);
        mesh.position.z = getRandomNumber(0, N);
        mesh.rotation.y = Math.PI / getRandomNumber(1, 360);
        
        mesh.castShadow = true;
        mesh.receiveShadow = true;

        mesh.userData.ID = ID;

        scene.add(mesh);
        modelsOnScene.push(mesh);
        mixersOnScene.push(mixer);
        
        ID++;
    });
}

function al()
{
    alert ("Все модели загружены и готовы к работе.");
}

function addMesh(name)
{
    var n = models.get(name).clone();

    n.position.y = 0;
    n.position.x = getRandomNumber(0, N);
    n.position.z = getRandomNumber(0, N);

    var box = new THREE.Box3();
    box.setFromObject(n);

    n.userData.box = box;

    const geometry = new THREE.BoxGeometry( 1, 1, 1 ); 
    
    const material = new THREE.MeshBasicMaterial( {color: 0x00ff00, wireframe: true} ); 
    const cube = new THREE.Mesh( geometry, material ); 
    scene.add( cube );

    cube.material.visible = false;

    var pos = new THREE.Vector3();
    box.getCenter(pos);
    var size = new THREE.Vector3();
    box.getSize(size);

    cube.position.copy(pos);
    cube.scale.set(size.x, size.y, size.z);

    n.userData.cube = cube;
    cube.userData.n = n;
    n.userData.ID = ID;

    scene.add(n);
    modelsOnScene.push(n);

    ID++;
}

function delMesh()
{
    if (selected != null)
    {
        for (var i = 0; i < modelsOnScene.length; i++)
        {
            if ( modelsOnScene[i].userData.ID == selected.userData.ID ) 
            {
                selected.userData.cube.material.visible = false;

                delete modelsOnScene[i].userData.cube;
                scene.remove(modelsOnScene[i]);
                selected = null;
                break;
            }
        }
    }
}

function getRandomNumber(min, max) {
    return Math.random() * (max - min) + min
}

//ниже всё по созданию ландшафта (часть 2)
function addCursor()
{
    // параметры цилиндра: диаметр вершины, диаметр основания, высота, число сегментов
    var cylinderGeometry = new THREE.CylinderGeometry(1.5, 0, 5, 64);
    var cylinderMaterial = new THREE.MeshLambertMaterial({ color: 0x888888 });
    cursor = new THREE.Mesh(cylinderGeometry, cylinderMaterial);
    cursor.visible = false;
    scene.add(cursor);
}

function addCircle()
{
    var segments = 64;

    var vertices = [];

    for (var i = 0; i <= segments; i++) {
        var theta = (i / segments) * Math.PI * 2;
        vertices.push(new THREE.Vector3(
            Math.cos(theta) * 1,   0.0,   Math.sin(theta) * 1));
    }
        
    // параметры: цвет, размер черты, размер промежутка
    const material = new THREE.LineBasicMaterial( {
        color: 0xffff00,
        linewidth: 1
    } );

    var lineGeometry = new THREE.BufferGeometry().setFromPoints(vertices);
    circle = new THREE.Line(lineGeometry, material);
    circle.scale.set(radius, 1, radius);
    circle.visible = false;
    scene.add(circle);
}

function onDocumentMouseScroll(event) 
{
    if (radius > 2)
        if (event.wheelDelta< 0) 
            radius -= 2;
    if (radius < 35)
        if (event.wheelDelta > 0) 
            radius += 2;
    circle.scale.set(radius, 1, radius);
}

function onDocumentMouseMove(event) 
{ 
    // получение экранных координат курсора мыши и приведение их к трёхмерным
    mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
    mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;
        
    // создание луча, исходящего из позиции камеры и проходящего сквозь позицию курсора мыши
    var vector = new THREE.Vector3(mouse.x, mouse.y, 1);
    vector.unproject(camera);

    var ray = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());

    // создание массива для хранения объектов, с которыми пересечётся луч 
    var intersects = ray.intersectObjects(targetList);

    if (brVal == true && selected == null)
    {
        // если луч пересёк какой-либо объект из списка targetList 
        if (intersects.length > 0)
        {
            if (cursor != null) 
            {
                cursor.position.copy(intersects[0].point);
                cursor.position.y += 2.5;
            }
            if (circle != null) 
            {
                circle.position.copy(intersects[0].point);
                circle.position.y = 0;

                for (var i = 0; i < circle.geometry.attributes.position.array.length / 3; i++)
                {
                    // получение позиции в локальной системе координат
                    var pos = new THREE.Vector3(circle.geometry.attributes.position.array[i*3], 
                        circle.geometry.attributes.position.array[i*3+1], circle.geometry.attributes.position.array[i*3+2]);
                    // нахождение позиции в глобальной системе координат
                    pos.applyMatrix4(circle.matrixWorld);


                    var x = Math.round(pos.x);
                    var z = Math.round(pos.z);

                    if (x > 0 && x < N && z > 0 && z < N)
                    {
                        var x1 = x;
                        var z1 = z;
                        var i1 = ((x1 * N) + z1)*3 + 1;

                        var y = geometry.attributes.position.array[i1];

                        circle.geometry.attributes.position.array[i*3+1] = y + 0.01;
                    } else circle.geometry.attributes.position.array[i*3+1] = 0;
                }
                circle.geometry.attributes.position.needsUpdate = true;
            }
        }
    } else {
        if (intersects.length > 0 && selected != null && press != null) {
            press.position.copy(intersects[0].point);
            press.userData.box.setFromObject(press);

            var pos = new THREE.Vector3();
            press.userData.box.getCenter(pos);

            press.userData.cube.position.copy(pos);
        }
    }

}

function onDocumentMouseDown(event) 
{ 
    if (brVal)
    {
        if (event.which == 1) bD = 1;
        if (event.which == 3) bD = -1;
    } else {
        // получение экранных координат курсора мыши и приведение их к трёхмерным
        mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
        mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;
            
        // создание луча, исходящего из позиции камеры и проходящего сквозь позицию курсора мыши
        var vector = new THREE.Vector3(mouse.x, mouse.y, 1);
        vector.unproject(camera);

        var ray = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());
        // создание массива для хранения объектов, с которыми пересечётся луч 
        var intersects = ray.intersectObjects(modelsOnScene, true);

        if (intersects.length > 0)
        {
            if (selected != null)
            {
                var s = intersects[0].object.parent;
                if (s == selected)
                {
                    selected.userData.cube.material.visible = false;
                    selected = null;
                    press = null;

                    //обнуление значения в шкалах gui
                    rotationSX.object.rx = 0;
                    rotationSY.object.ry = 0;
                    rotationSY.object.ry = 0;

                    scaleSX.object.sx = 0;
                    scaleSY.object.sy = 0;
                    scaleSY.object.sy = 0;
                } else{
                    selected.userData.cube.material.visible = false;
                    selected = intersects[0].object.parent;
                    selected.userData.cube.material.visible = true;
                    press = selected;

                    //обнуление значения в шкалах gui
                    rotationSX.object.rx = 0;
                    rotationSY.object.ry = 0;
                    rotationSY.object.ry = 0;

                    scaleSX.object.sx = 0;
                    scaleSY.object.sy = 0;
                    scaleSY.object.sy = 0;
                }
            } else {
                selected = intersects[0].object.parent;
                selected.userData.cube.material.visible = true;
                press = selected;

                //присваивание значения в шкалах gui от выбранного объекта
                rotationSX.object.rx = selected.rotation._x;
                rotationSY.object.ry = selected.rotation._y;
                rotationSZ.object.rz = selected.rotation._z;
                
                scaleSX.object.sx = selected.scale.x;
                scaleSY.object.sy = selected.scale.x;
                scaleSY.object.sy = selected.scale.x;
            }
        } else 
        if (selected != null && selected.userData.pl == undefined)
        {
            selected.userData.cube.material.visible = false;
            selected = null;
            press = null;

            //обнуление значения в шкалах gui
            rotationSX.object.rx = 0;
            rotationSY.object.ry = 0;
            rotationSY.object.ry = 0;

            scaleSX.object.sx = 0;
            scaleSY.object.sy = 0;
            scaleSY.object.sy = 0;
        }
    }
}

function onDocumentMouseUp(event) 
{ 
    if (brVal) bD = 0;
    press = null;
}

function create_relief()
{
    if (brVal == true)
    {
        for (var i = 0; i < geometry.attributes.position.array.length / 3; i++)
        {
            var x2 = geometry.attributes.position.array[i*3]; //x
            var z2 = geometry.attributes.position.array[i*3+2]; //z
            var r = radius;
            var x1 = cursor.position.x;
            var z1 = cursor.position.z;

            var h = r * r - (((x2 - x1) * (x2 - x1)) + ((z2 - z1) * (z2 - z1)));

            if (h > 0) 
            {
                geometry.attributes.position.array[i*3+1] += Math.sqrt(h) * bD / 20; //y
            }
        }
        
        geometry.computeVertexNormals(); //пересчёт нормалей      
        geometry.attributes.position.needsUpdate = true; //обновление вершин
        geometry.attributes.normal.needsUpdate = true; //обновление нормалей
        geometry.attributes.uv.needsUpdate = true;
    }
}

//ниже всё по интерфейсу (часть 3)
function GUI()
{
    // массив переменных, ассоциированных с интерфейсом  
    var params = 
    {
        sx: 0,
        sy: 0,
        sz: 0,
        rx: 0,
        ry: 0,
        rz: 0,
        brush: false,
        addHouse: function() { addMesh('hous') },
        addGrade: function() { addMesh('grade') },
        addCactus: function() { addMesh('cactus') },
        addPine: function() { addMesh('pine') },
        addFern: function() { addMesh('fern') },
        addParrot: function() { loadAnimatedModel('./models/animals/Parrot.glb', 0.2)},
        addFlamingo: function() { loadAnimatedModel('./models/animals/Flamingo.glb', 0.2)},
        delete: function() { delMesh() }
    };

    // создание вкладки
    var folder1 = gui.addFolder('Scale');
    scaleSX = folder1.add(params, 'sx').min(0).max(100).step(1).listen();
    scaleSY = folder1.add(params, 'sy').min(0).max(100).step(1).listen();
    scaleSZ = folder1.add(params, 'sz').min(0).max(100).step(1).listen();
    // при запуске программы папка будет открыта
    folder1.open();

    // описание действий совершаемых при изменении ассоциированных значений  
    scaleSX.onChange(function(value) {
        var lastScaleValue = selected.scale.x;
        var r = value; 
        value = value - lastScaleValue;
        lastScaleValue = r;
        selected.scale.x += value;

        selected.userData.box.setFromObject(selected);
        
        var pos = new THREE.Vector3();
        selected.userData.box.getCenter(pos);
        selected.userData.cube.position.copy(pos);

        var size = new THREE.Vector3();
        selected.userData.box.getSize(size);

        selected.userData.cube.scale.set(size.x, size.y, size.z);
     });
    scaleSY.onChange(function(value) {
        var lastScaleValue = selected.scale.y;
        var r = value; 
        value = value - lastScaleValue;
        lastScaleValue = r;
        selected.scale.y += value;

        selected.userData.box.setFromObject(selected);
        
        var pos = new THREE.Vector3();
        selected.userData.box.getCenter(pos);
        selected.userData.cube.position.copy(pos);

        var size = new THREE.Vector3();
        selected.userData.box.getSize(size);

        selected.userData.cube.scale.set(size.x, size.y, size.z);
     });
    scaleSZ.onChange(function(value) {
        var lastScaleValue = selected.scale.z;
        var r = value; 
        value = value - lastScaleValue;
        lastScaleValue = r;
        selected.scale.z += value;

        selected.userData.box.setFromObject(selected);
        
        var pos = new THREE.Vector3();
        selected.userData.box.getCenter(pos);
        selected.userData.cube.position.copy(pos);

        var size = new THREE.Vector3();
        selected.userData.box.getSize(size);

        selected.userData.cube.scale.set(size.x, size.y, size.z);
     });

    // создание вкладки
    var folder2 = gui.addFolder('Rotation');
    rotationSX = folder2.add(params, 'rx').min(0).max(359).step(1).listen();
    rotationSY = folder2.add(params, 'ry').min(0).max(359).step(1).listen();
    rotationSZ = folder2.add(params, 'rz').min(0).max(359).step(1).listen();
    folder2.open();

    var lastRotValueX = 0;
    var lastRotValueY = 0;
    var lastRotValueZ = 0;

    rotationSX.onChange(function(value) {
        var r = value;
        value = value - lastRotValueX;
        lastRotValueX = r;
        selected.rotation.x += value * (Math.PI / 180);

        selected.userData.box.setFromObject(selected);
        var pos = new THREE.Vector3();
        selected.userData.box.getCenter(pos);
        selected.userData.cube.position.copy(pos);

        selected.userData.cube.rotation.x = selected.rotation._x;
     });
    rotationSY.onChange(function(value) {
        var r = value;
        value = value - lastRotValueY;
        lastRotValueY = r;
        selected.rotation.y += value * (Math.PI / 180);

        selected.userData.box.setFromObject(selected);
        var pos = new THREE.Vector3();
        selected.userData.box.getCenter(pos);
        selected.userData.cube.position.copy(pos);

        selected.userData.cube.rotation.y = selected.rotation._y;
     });
    rotationSZ.onChange(function(value) {
        var r = value;
        value = value - lastRotValueZ;
        lastRotValueZ = r;
        selected.rotation.z += value * (Math.PI / 180);

        selected.userData.box.setFromObject(selected);
        var pos = new THREE.Vector3();
        selected.userData.box.getCenter(pos);
        selected.userData.cube.position.copy(pos);

        selected.userData.cube.rotation.z = selected.rotation._z;
     });

    // добавление кнопок, при нажатии которых будут вызываться функции addHouse
    var folder2 = gui.addFolder('add');
    for (var i = 0; i < guiModelsAdd.length; i++)
    {
        folder2.add(params, guiModelsAdd[i][0]).name(guiModelsAdd[i][1])
    }
    folder2.open();

    gui.add(params, 'delete').name("delete");

    // добавление чекбокса с именем brush
    var brushVisible = gui.add(params, 'brush').name('brush').listen();
    brushVisible.onChange(function(value) 
    { 
        if (selected != null)
        {
            selected.userData.cube.material.visible = false;
            selected = null;
            press = null;
        }

        brVal = value;
        cursor.visible = value;
        circle.visible = value;
    });
        
    // при запуске программы интерфейс будет раскрыт
    gui.open();

}

//ниже всё по созданию сцены (небо, камера и тд) (часть 0)
function init() 
{
    console.log('init');
    // получение ссылки на блок html-страницы
    container = document.getElementById('container');
    // создание сцены
    scene = new THREE.Scene();

    // КАМЕРА
    camera = new THREE.PerspectiveCamera(50, window.innerWidth / window.innerHeight, 1, 4000);    
    camera.position.set(N/2, N, N * 1.4);
    camera.lookAt(new THREE.Vector3(N/2, 0, N/2));  

    // РЕНДЕРЕР
    renderer = new THREE.WebGLRenderer( { antialias: false } );
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setClearColor(0x87CEEB, 1);

    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFShadowMap;

    container.appendChild(renderer.domElement);

    // добавление обработчика события изменения размеров окна
    window.addEventListener('resize', onWindowResize, false);

    // СВЕТ
    var spotlight = new THREE.SpotLight(0xffffff, 1);
    spotlight.position.set(N, N, N);

    spotlight.shadow.mapSize.width = 512;
    spotlight.shadow.mapSize.height = 512;
    spotlight.shadow.camera.near = 0.5;
    spotlight.shadow.camera.far = 5000;

    spotlight.castShadow = true; 

    // добавление источника в сцену
    scene.add(spotlight);
    scene.add(new THREE.AmbientLight(0xFDFCEB, 0.5)  );


    renderer.domElement.addEventListener('mousedown', onDocumentMouseDown, false);
    renderer.domElement.addEventListener('mouseup', onDocumentMouseUp, false);
    renderer.domElement.addEventListener('mousemove', onDocumentMouseMove, false);
    renderer.domElement.addEventListener('wheel', onDocumentMouseScroll, false);
    renderer.domElement.addEventListener("contextmenu", function (event) {event.preventDefault();});

    addPlane();
    create_sky("./pics/fantastic-cloudscape.jpg", 2000);

    addCursor();
    addCircle();

    for (var i = 0; i < loadModels.length; i ++)
    {
        loadModel(loadModels[i][0], loadModels[i][1], loadModels[i][2], loadModels[i][3], loadModels[i][4], loadModels[i][5]);
    }

    /*for (var j = 0; j < loadAnimModels.length; j ++)
    {
        loadAnimatedModel(loadAnimModels[j][0], loadAnimModels[j][1], loadAnimModels[j][2]);
    }*/

    GUI();
}

function onWindowResize() 
{
    // изменение соотношения сторон для виртуальной камеры
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    // изменение соотношения сторон рендера
    renderer.setSize(window.innerWidth, window.innerHeight);
}

function animate() 
{
    // добавление функции на вызов при перерисовке браузером страницы 
    requestAnimationFrame(animate);

    if (mixersOnScene.length > 0)
    {
        var delta = clock.getDelta();
        //if(mixer) mixer.update( delta );

        for (var i = 0; i < mixersOnScene.length; i++)
        {
            if(mixersOnScene[i]) mixersOnScene[i].update(delta);
        }
    }

    if (bD != 0) create_relief();

    render();   
}

function render() 
{
    // рисование кадра
    renderer.render(scene, camera);
}

//создание плоскости
function addPlane()
{
    var vertices = []; //массив вершин
    var faces = []; //массив индексов
    var uvs = []; //массив текстурных карт

    geometry = new THREE.BufferGeometry();

    for (var y = 0; y < N; y++)
    for (var u = 0; u < N; u++)
    {
        vertices.push(y, 0, u);
        uvs.push(y/(N-1), u/(N-1));
    }

    for (var y = 0; y < N - 1; y++)
    for (var u = 0; u < N - 1; u++)
    {
        faces.push(y + u * N, (y + 1) + u * N, (y + 1) + (u + 1) * N);
        faces.push(y + u * N, (y + 1) + (u + 1) * N, y + (u + 1) * N);
    }

    geometry.setAttribute( 'position', new THREE.Float32BufferAttribute( vertices, 3 ));
    geometry.setIndex( faces );
    geometry.setAttribute( 'uv', new THREE.Float32BufferAttribute( uvs, 2 ));

    geometry.computeVertexNormals();

    // режим повторения текстуры
    tex.wrapS = tex.wrapT = THREE.RepeatWrapping; 
    // повторить текстуру 10 на 10 раз
    tex.repeat.set(3, 3);

    var material = new THREE.MeshLambertMaterial({
        map: tex,
        wireframe: false,
        side: THREE.DoubleSide
    });

    var plane = new THREE.Mesh(geometry, material);
    plane.position.set(0.0, 0.0, 0.0);

    plane.receiveShadow = true;
    //plane.castShadow = true; 

    //чтобы не возникало ошибки, когда программа думает что сфера относится к загруженным объектам
    plane.userData.pl = null;

    // добавление плоскости (ландшафта) в массив
    targetList.push(plane);

    // добавление объекта в сцену     
    scene.add(plane);
}

//создание неба
function create_sky(texture, rad)
{
    // создание геометрии для сферы
    var sGeometry = new THREE.SphereGeometry(rad, 50, 50);

    // загрузка текстуры
    var tex = loader.load(texture);
    tex.minFilter = THREE.NearestFilter;

    // создание материала
    var material = new THREE.MeshBasicMaterial({
        map: tex,
        side: THREE.DoubleSide
    });
    
    // создание объекта
    var sphere = new THREE.Mesh(sGeometry, material);

    sphere.position.set(0, 0, 0);

    //чтобы не возникало ошибки, когда программа думает что сфера относится к загруженным объектам
    sphere.userData.pl = null;
    
    // размещение объекта в сцене
    scene.add(sphere);
}

