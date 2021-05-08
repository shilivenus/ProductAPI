# Product API

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
      <ul>
        <li><a href="#assumption">EndPoint</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
    </li>
    <li><a href="#result">Result</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

This is a .Net Core Web Api using EF and SQLite DB. This API has ability to CRUD Products and Product Options. Product and Product Option have one to many relationship. 
ExceptionMiddleware is doing global error handling.

### Built With
* [.Net 5]

#### EndPoint

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

<!-- GETTING STARTED -->
## Getting Started

1. Build solution and run the Web api.
2. Using Postman or Swagger to test endpoints.

<!-- Result EXAMPLES -->
## Result

After steps in getting start section. Api should give correct data and response status code back.

<!-- CONTACT -->
## Contact

Li Shi - shilimonash@gmail.com