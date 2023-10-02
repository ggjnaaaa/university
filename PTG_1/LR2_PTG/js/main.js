// ссылка на блок веб-страницы, в котором будет отображаться графика
var container;

// переменные: камера, сцена и отрисовщик
var camera, scene, renderer;

// создание загрузчика текстур
var loader = new THREE.TextureLoader();

var clock = new THREE.Clock();
var keyboard = new THREEx.KeyboardState();

var planets = []; //массив планет
var satellites = []; //массив спутников
var satellitesLine = []; //массив орбит спутников

var pressed = 0; //хранит значение нажатой клавиши

var cameraCurrentLook = new THREE.Vector3(0, 0, 0); 

var switchSpeed = 3.0;
var rotationSpeed = 1.0;

init();

animate();

function init()
{
    container = document.getElementById('container');
    scene = new THREE.Scene();

    // КАМЕРА
    camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 1, 4000);    
    camera.position.set(0, 700, 1600);
    camera.lookAt(new THREE.Vector3(0, 0, 0));  

    //РЕНДЕРЕР
    renderer = new THREE.WebGLRenderer( { antialias: false } );
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setClearColor(0x000000ff, 1);

    container.appendChild(renderer.domElement);

    window.addEventListener('resize', onWindowResize, false);

    //СВЕТ
    var spotlight = new THREE.PointLight(0xffffff);
    spotlight.position.set(0, 0, 0);
    scene.add(spotlight);

    //              текстура       радиус
    create_stars("pics/sunmap.jpg", 140);
    create_stars("pics/starmap.jpg", 2000);

    //                   текстура,                       карта высот,        радиус, дистанция, скорости(поворот вокруг солнца/своей оси)
    create_planet("pics/mercury/mercurymap.jpg", "pics/mercury/mercurybump.jpg", 20, 200, 0.5, 0.75);
    create_planet("pics/venus/venusmap.jpg", "pics/venus/venusbump.jpg", 40, 400, 0.35, 0.62);
    create_planet("pics/earth/earthmap1k.jpg", "pics/earth/earthbump1k.jpg", 60, 600, 0.25, 0.5);
    create_planet("pics/mars/marsmap1k.jpg", "pics/mars/marsbump1k.jpg", 50, 800, 0.2, 0.7);

    //                        текстура,                         карта высот,         радиус, дистанция, скорости(поворот вокруг своей оси/планеты), номер планеты
    create_satellites("pics/earth/moon/moonmap1k.jpg", "pics/earth/moon/moonbump1k.jpg", 10, 80, 2, 0.7, 3);
}

//создание_звёзд(текстура, радиус)
function create_stars(texture, rad)
{
    // создание геометрии для сферы
    var geometry = new THREE.SphereGeometry(rad, 50, 50);

    // загрузка текстуры
    var tex = loader.load(texture);
    tex.minFilter = THREE.NearestFilter;

    // создание материала
    var material = new THREE.MeshBasicMaterial({
        map: tex,
        side: THREE.DoubleSide
    });
    
    // создание объекта
    var sphere = new THREE.Mesh(geometry, material);

    sphere.position.set(0, 0, 0);
    
    // размещение объекта в сцене
    scene.add(sphere);
}

//создание_планеты(текстура, радиус, дистанция от солнца, скорость поворота вокруг солнца, скорость поворота вокруг своей оси)
function create_planet(texture, bump, rad, dist, s1, s2)
{
    // создание геометрии для сферы
    var geometry = new THREE.SphereGeometry(rad, 32, 32);

    // загрузка текстуры
    var tex = loader.load(texture);
    tex.minFilter = THREE.NearestFilter;

    // загрузка карты рельефа
    var bump = loader.load(bump);

    // назначение карты рельефа и её масштабирования
    var material = new THREE.MeshPhongMaterial({
        map: tex,
        bumpMap: bump,
        bumpScale: 0.5,
        side: THREE.DoubleSide
    });
    
    // создание объекта
    var sphere = new THREE.Mesh(geometry, material);

    sphere.position.set(dist, 0, 0);
    
    // размещение объекта в сцене
    scene.add(sphere);
    
    //объект планета с данными о планете
    var planet = {
        model: sphere,
        orbit: dist,
        s1: s1,
        s2: s2,
        a1: 0.0,
        a2: 0.0,
        rad: rad
    }
    
    //добавление планеты в массив с планетами
    planets.push(planet);

    //является ли спутником
    var sat = false;
    lines(dist, sat, null);
}

//создание_спутника(текстура, радиус, дистанция от планеты, скорости(поворот вокруг своей оси/планеты), номер планеты)
function create_satellites(texture, bump, rad, dist, s1, s2, numPlanet)
{
    // создание геометрии для сферы
    var geometry = new THREE.SphereGeometry(rad, 32, 32);

    // загрузка текстуры
    var tex = loader.load(texture);
    tex.minFilter = THREE.NearestFilter;

    // загрузка карты рельефа
    var bump = loader.load(bump);

    // назначение карты рельефа и её масштабирования
    var material = new THREE.MeshPhongMaterial({
        map: tex,
        bumpMap: bump,
        bumpScale: 0.5,
        side: THREE.DoubleSide
    });
    
    // создание объекта
    var sphere = new THREE.Mesh(geometry, material);

    sphere.position.set(planets[numPlanet-1].orbit + dist, 0, 0);
    
    // размещение объекта в сцене
    scene.add(sphere);
    
    //объект планета с данными о планете
    var satellite = {
        model: sphere,
        orbit: dist,
        s1: s1,
        s2: s2,
        a1: 0.0,
        a2: 0.0,
        rad: rad
    }
    
    //добавление спутника в массив со спутниками
    satellites[numPlanet-1] = satellite;

    var sat = true;
    lines(dist, sat, numPlanet);
}

function onWindowResize() 
{
    // изменение соотношения сторон для виртуальной камеры
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    // изменение соотношения сторон рендера
    renderer.setSize(window.innerWidth, window.innerHeight);
}

// в этой функции можно изменять параметры объектов и обрабатывать действия пользователя
function animate() 
{
    var delta = clock.getDelta();//возвращает время, прошедшее с момента предыдущего вызова этой функции

    for (var i = 0; i < planets.length; i++)
    {
        //создание набора матриц
        var m = new THREE.Matrix4();
        var m1 = new THREE.Matrix4();
        var m2 = new THREE.Matrix4();

        planets[i].a1 += planets[i].s1 * delta;
        planets[i].a2 += planets[i].s2 * delta;

        // создание матрицы поворота (вокруг оси Y) в m1 и матрицы перемещения в m2           
        m1.makeRotationY(planets[i].a1);
        m2.setPosition(new THREE.Vector3(planets[i].orbit, 0, 0));

        // запись результата перемножения m1 и m2 в m           
        m.multiplyMatrices(m1, m2);

        m1.makeRotationY(planets[i].a2);       
        m.multiplyMatrices(m, m1);

        // установка m в качестве матрицы преобразований объекта object            
        planets[i].model.matrix = m;
        planets[i].model.matrixAutoUpdate = false;

        if (satellites[i] != null)
        {
            //создание набора матриц
            var sm = new THREE.Matrix4();
            var sm1 = new THREE.Matrix4();
            var sm2 = new THREE.Matrix4();

            satellites[i].a1 += satellites[i].s1 * delta;
            satellites[i].a2 += satellites[i].s2 * delta;

            // создание матрицы поворота (вокруг оси Y) в m1 и матрицы перемещения в m2           
            sm1.makeRotationY(satellites[i].a1);
            sm2.setPosition(new THREE.Vector3(satellites[i].orbit, 0, 0));

            sm.multiplyMatrices(sm2, sm1);

            sm1.makeRotationY(satellites[i].a2);

            // запись результата перемножения m1 и m2 в m           
            sm.multiplyMatrices(sm1, sm);

            sm1.copyPosition(planets[i].model.matrix);
            // получение позиции из матрицы позиции
            var pos = new THREE.Vector3(0, 0, 0);
            pos.setFromMatrixPosition(sm1);
                   
            sm.multiplyMatrices(sm1, sm);


            // установка m в качестве матрицы преобразований объекта object            
            satellites[i].model.matrix = sm;
            satellites[i].model.matrixAutoUpdate = false;

            satellitesLine[i].position.copy(pos);
        }
    }

    key(delta);

    // добавление функции на вызов при перерисовке браузером страницы 
    requestAnimationFrame(animate);

    render();   
}

function render() 
{ 
    // рисование кадра
    renderer.render(scene, camera);
}

//проверка нажатия на кнопку
function key(delta)
{
    if (keyboard.pressed("0")) 
    {
        pressed = 0;
        rotationSpeed = 1.0;
        switchSpeed = 3.0;
    }

    if (keyboard.pressed("1")) 
    {
        pressed = 1;
        rotationSpeed = 1.0;
        switchSpeed = 3.0;
    }

    if (keyboard.pressed("2")) 
    {
        pressed = 2;
        rotationSpeed = 1.0;
        switchSpeed = 3.0;
    }

    if (keyboard.pressed("3")) 
    {
        pressed = 3;
        rotationSpeed = 1.0;
        switchSpeed = 3.0;
    }

    if (keyboard.pressed("4")) 
    {
        pressed = 4;
        rotationSpeed = 1.0;
        switchSpeed = 3.0;
    }

    if (pressed == 0)
    {
        if (rotationSpeed < 10) rotationSpeed += delta * 2.0;
        if (switchSpeed < 10) rotationSpeed += delta * 2.0;

        // установка позиции камеры
        camera.position.lerp(new THREE.Vector3(0, 700, 1600), delta * switchSpeed);

        // установка точки, на которую камера будет смотреть 
        cameraCurrentLook.lerp(new THREE.Vector3(0, 0, 0), delta * rotationSpeed);

        camera.lookAt(cameraCurrentLook); 
    } else

    if (pressed != 0)
    {
        if (rotationSpeed < 10) rotationSpeed += delta * 2.0;
        if (switchSpeed < 10) rotationSpeed += delta * 2.0;

        var p = planets[pressed - 1];

        // получение матрицы позиции из матрицы объекта 
        var m = new THREE.Matrix4();
        m.copyPosition(p.model.matrix);

        // получение позиции из матрицы позиции
        var pos = new THREE.Vector3(0, 0, 0);
        pos.setFromMatrixPosition(m);

        var camPos = new THREE.Vector3(pos.x + (p.rad * 2) * Math.cos(Math.PI/4 - p.a1),
                                         50, 
                                         pos.z + (p.rad * 2) * Math.sin(Math.PI/4 - p.a1));

        // установка позиции камеры
        camera.position.lerp(camPos, delta * switchSpeed);
        cameraCurrentLook.lerp(pos, delta * rotationSpeed);
        
        // установка точки, на которую камера будет смотреть
        camera.lookAt(cameraCurrentLook); 
    }
}

//создание орбит
function lines(dist, sat, numPlanet)
{
    var vertices = [];

    for (var i = 0; i <= 50; i++) {
        var theta = (i / 50) * Math.PI * 2;
        vertices.push(new THREE.Vector3(
            Math.cos(theta) * dist,   0.0,   Math.sin(theta) * dist));
    }
        
    // параметры: цвет, размер черты, размер промежутка
    var lineMaterial = new THREE.LineDashedMaterial({
        color: 0xcccc00,
        dashSize: 20,
        gapSize: 20 
    });

    var lineGeometry = new THREE.BufferGeometry().setFromPoints(vertices);
    var line = new THREE.Line(lineGeometry, lineMaterial);
    line.computeLineDistances();
    scene.add(line);

    //если спутник, то добавляет в массив орбит спутников
    if (sat)
    satellitesLine[numPlanet - 1] = line;
}