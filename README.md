# Eventify

Eventify es una aplicación web que está diseñada para facilitar el proceso de selección de comidas y bebidas para cualquier evento y obtener una lista de compras optimizada.

La misma permite manejar dos tipos de usuarios, el Usuario final y el Comercio. El usuario final podrá utilizar la app para armar listas de compras para sus eventos obteniendo las mejores ofertas y con el menor recorrido posible en base a su ubicación y los comercios podrán obtener visibilidad en la app mediante las publicaciones de sus ofertas en la misma.

Eventify consta de un componente [API Backend](https://github.com/Sacorv/AsistenteCompras_BE/tree/v1.1.0-RC1) que disponibiliza todos los servicios necesarios y por otro lado, un [Componente Frontend](https://github.com/alexisjv/Eventify_FE/tree/master) que consume dichos servicios.


## Hosting

La aplicación web se encuentra publicada en Firebase, mientras que la API se encuentra publicada en Microsoft Azure. A continuación, se listan las URLs para probar cada uno de ellos:

* Aplicación Web: https://eventify-tpi.web.app/

* API: https://eventify-api.azurewebsites.net/api/Evento/eventos


## Tecnologías

- C# .NET 6.0

- Entity Framework Core 7.0

- SQL Server 12.0

- Angular 16.0.5

- Typescript 4.9.4


### Diagrama de Arquitectura

![diagrama](./documents/diagrama-eventify.png)


## Build y Run

1. Clonar el repositorio desde:
- WEB: https://github.com/alexisjv/Eventify_FE/tree/develop
- API: https://github.com/Sacorv/AsistenteCompras_BE/tree/develop

##### Para el componente WEB

1. Una vez clonado el proyecto, ejecutar el comando `npm install` en la terminal de la IDE
2. Levantar el poroyecto ejecutando `ng serve -o`
4. URL por default https://localhost:7292/api/

##### Para el componente API

1. Crear una Base de datos local SQL Server vacía
2. Ejecutar los scripts que se encuentran en la carpeta `database`
3. Configurar en el archivo appSettings.json la conexión a la base de datos. Colocar la llave: [conexión a la DB creada](https://github.com/Sacorv/AsistenteCompras_BE/blob/develop/AsistenteCompras_API/AsistenteCompras_API/appsettings.json) 
4. Ejecutar el proyecto
5. URL por default https://localhost:7292/swagger/index.html

## Collection de Postman

Se disponibilizan link de la collection con todos los servicios expuestos por el API:

- **Componente API**: https://api.postman.com/collections/15615615-b4f5f053-236f-4b3f-8a6c-02a9512c736b?access_key=PMAT-01H4Z1B3BQM583Z0X3HASE6TW7

