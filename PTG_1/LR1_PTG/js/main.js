// ссылка на блок веб-страницы, в котором будет отображаться графика
var container;

// глобальная переменная для хранения карты высот
var imagedata;

var N = 256;

// переменные: камера, сцена и отрисовщик
var camera, scene, renderer;

// создание загрузчика текстур
var loader = new THREE.TextureLoader();
// загрузка текстуры grasstile.jpg из папки pics
var tex = loader.load('pics/grasstile.jpg');

// функция инициализации камеры, отрисовщика, объектов сцены и т.д.
init();

// обновление данных по таймеру браузера
animate();

// в этой функции можно добавлять объекты и выполнять их первичную настройку
function init() 
{
    // получение ссылки на блок html-страницы
    container = document.getElementById('container');
    // создание сцены
    scene = new THREE.Scene();

    // CAMERA
    camera = new THREE.PerspectiveCamera(50, window.innerWidth / window.innerHeight, 1, 4000);    
    camera.position.set(N/2, N/2, N * 1.35);
    camera.lookAt(new THREE.Vector3(N/2, 0, N/2));  

    // RENDERER
    renderer = new THREE.WebGLRenderer( { antialias: false } );
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setClearColor(0x87CEEB, 1);

    container.appendChild(renderer.domElement);

    // добавление обработчика события изменения размеров окна
    window.addEventListener('resize', onWindowResize, false);


    // СВЕТ
    var spotlight = new THREE.PointLight(0xffffff);
    spotlight.position.set(N*4, N, N);
    scene.add(spotlight);

    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    var img = new Image();

    // загрузка изображения с картой высот
    img.src = 'pics/plateau.jpg'; 

    img.onload = function()
    {
        canvas.width = img.width;
        canvas.height = img.height;
        context.drawImage(img, 0, 0);
        imagedata = context.getImageData(0, 0, img.width, img.height);

        // пользовательская функция генерации ландшафта
        addObjects();
    }
}

function getPixel(x, y) 
{
    var position = (x + imagedata.width * y) * 4;
    return imagedata.data[position];
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
    requestAnimationFrame(animate);
    render();   
}

function render() 
{
    renderer.render(scene, camera);
}

function addObjects()
{
    var vertices = []; //массив вершин
    var faces = []; //массив индексов
    var uvs = []; //массив текстурных карт

    var geometry = new THREE.BufferGeometry();

    for (var y = 0; y < N; y++)
    for (var u = 0; u < N; u++)
    {
        // получение цвета пикселя в десятом столбце десятой строки изображения
       var h = getPixel(y, u);  

        vertices.push(y, h/10, u);
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

    // повторенияе текстуры
    tex.wrapS = tex.wrapT = THREE.RepeatWrapping; 
    tex.repeat.set(3, 3);

    var material = new THREE.MeshLambertMaterial({
        map: tex,
        wireframe: false,
        side: THREE.DoubleSide
    });

    var mesh = new THREE.Mesh(geometry, material);
    mesh.position.set(0.0, 0.0, 0.0);
    
    // добавление объекта в сцену     
    scene.add(mesh);
}

