1. This project was developed usuing VS 2022, Sql Server 2022. After installing these 2 in your PC then run the project. Or you can do it using DOcker.

2. To test thge API install postman. Then send a post request from POSTMAN set the address https://localhost:7165/api/Movies and 
send an object using json format. select body, json content. 
For ex, For creating a new movie , use POST request:
[
    
    { "Title": "Avatar2", "Genre": "Sci-Fi", "ReleaseDate": "2012-12-18T00:00:00Z" }
]

To update a record send PUT req using https://localhost:7165/api/Movies/100 
EX: 
    {
    "Id": 1007,
    "Title": "Avatar 005",
    "Genre": "Sci-Fi",
    "ReleaseDate": "2009-12-18T00:00:00"
    }

To get all the Movie records. Uset GET. https://localhost:7165/api/Movies

To Delete a Movie, use DELETE req. https://localhost:7165/api/Movies/1004





** Project Description**


This Project demonstrates how to build a RESTful API with CRUD functionality:

GET: Retrieve all movies or a specific movie by ID.
POST: Add a new movie or a list of movies.
PUT: Update an existing movie by ID.
DELETE: Remove a movie by ID.