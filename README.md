# BlackJackApi

http://alvarfs-001-site1.qtempurl.com/swagger/index.html

## Esquema de Arquitectura

- **Modelos (`Models`)**: Definen la estructura de los datos y representan las entidades principales (por ejemplo, `User`, `UserDTO`, `DeckResponse`, `CardResponse`, etc.).
- **Controladores (`Controllers`)**: Gestionan las peticiones HTTP y contienen la lógica principal para interactuar con los modelos y devolver respuestas adecuadas. Ejemplos: `CardsController` y `UserController`.
- **Persistencia**: Se utiliza Entity Framework Core junto a SQLite para la gestión de usuarios y sus datos.
- **Configuración y entrada (`Program.cs`)**: Configura los servicios, CORS, Swagger y el pipeline principal de la API.

## Explicación detallada del código y la arquitectura MVC

### Modelos
- **User & UserDTO**: Representan los usuarios del sistema, diferenciando entre la entidad completa y la información expuesta al cliente.
- **DeckResponse, CardResponse, Card**: Modelan la información recibida de la API externa de barajas de cartas.
- **DeckReturn, CardReturn**: Estructuras de datos para devolver información procesada al cliente.
- **UserContext**: Contexto de Entity Framework para la gestión de usuarios en la base de datos.

### Controladores
- **CardsController**: Gestiona la interacción con la API externa de cartas. Permite crear mazos, robar cartas y mezclar mazos. Cada método expone un endpoint REST y devuelve respuestas estructuradas.
- **UserController**: Gestiona los usuarios. Permite crear, consultar, actualizar, autenticar y eliminar usuarios. Utiliza el contexto de base de datos para persistencia y expone endpoints REST claros.

### Flujo MVC
- El usuario realiza una petición HTTP a un endpoint.
- El controlador correspondiente procesa la petición, interactúa con los modelos (y la base de datos si es necesario) y retorna una respuesta.
