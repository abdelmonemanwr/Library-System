# 📚 Library Management System API

Welcome to the **Library Management System API**! This project is a feature-rich backend solution built with **ASP.NET Core**, designed to manage library operations efficiently. Dive into the details below to understand how to set it up and start using it. 🚀

---

## 🌟 Features

### Core Functionalities
- **📖 Book Management**: Create, update, delete, and fetch books.
- **🔒 User Authentication**: Secure endpoints with user login, registration, and logout functionality.
- **🔍 Advanced Search**: Filter books by title, author, or genre.
- **📊 Pagination & Sorting**: Handle large datasets with ease.
- **🛡️ Role-Based Access**: Different roles for Admins and Users for secure access.

### Technical Highlights
- Built with the latest **ASP.NET Core v8.0**.
- Database operations powered by **Entity Framework Core**.
- **Caching** for quick access to frequently requested data.
- Fully documented with **Swagger**.
- Comprehensive **Unit Tests** included.

---

## 🛠️ Prerequisites

Ensure you have the following installed before starting:
- ✅ **.NET SDK**
- ✅ **SQL Server**
- ✅ **Git**
- ✅ **Visual Studio 2022** or **Visual Studio Code**

---

## 🚀 Getting Started

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/abdelmonemanwr/Library-System.git
   cd Library-System
   ```

2. **Set Up the Database**:
   - Run migrations to create the database schema:
     ```bash
     dotnet ef database update
     ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```
   The API will be accessible at:
   - `https://localhost:7176` (HTTPS)
   - `http://localhost:5106` (HTTP)

4. **Explore Swagger Documentation**:
   Head to `https://localhost:7176/swagger` to view and test all API endpoints.

---

## 🔗 API Endpoints

### 📜 Authentication
- `POST /api/account/login` - User login.
- `POST /api/account/register` - User registration.
- `POST /api/account/logout` - User logout.
- `GET /api/account/check-existing-email` - Check if an email is already registered.

### 📖 Books
- `POST /api/books` - Add a new book (Admin only).
- `PUT /api/books/{id}` - Update an existing book (Admin only).
- `DELETE /api/books/{id}` - Delete a book by ID (Admin only).
- `GET /api/books` - Retrieve books with pagination, filtering, and sorting.

### 📘 Borrowing
- `POST /api/borrowing/{bookId}` - Borrow a book.
- `GET /api/borrowing` - View books borrowed by the logged-in user.
- `POST /api/borrowing/return/{bookId}` - Return a borrowed book.

---

## ✅ Testing

Unit tests are included in the project:

1. **Run All Tests**:
   ```bash
   dotnet test
   ```

2. The test suite is located in the `Library.Management.System.Tests` directory.

---

## 📂 Project Structure

The solution consists of two projects:

### 1. **API Project**
- **Controllers**: Handle HTTP requests and responses.
- **Services**: Core business logic.
- **Repositories**: Handle data access with Entity Framework Core.
- **Models**: Define the data structures.
- **DTOs**: Manage request and response objects.
- **Helpers & Extensions**: Additional utilities and extensions.
- **Migrations**: Database migration files.
- **Unit of Work**: Ensures a clean data access layer.

### 2. **Test Project**
- Contains **unit tests** for validating the core logic and API behavior.

---

## 🛠️ Technologies Used

- **ASP.NET Core** - Web API Framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Database solution
- **AutoMapper** - Object-to-object mapping
- **Swagger** - API documentation
- **XUnit** - Unit testing framework
- **Moq** - Mocking for unit tests

---

## 🙋‍♂️ Author

**Abdalmonem Anwar**  
For inquiries or feedback, feel free to reach out:
- 📧 Email: abdelmonemanwr7777@gmail.com
- 🌐 LinkedIn: [Abdalmonem Anwar](https://www.linkedin.com/in/abdalmonem-anwar-73710a1ba/)

---

## 📜 License

This project is licensed under the **MIT License**. Check the `LICENSE` file for more details.

---

**Thank you for exploring the Library Management System API! 🙌**

**Happy coding! 🎉**
