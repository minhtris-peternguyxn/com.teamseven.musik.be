---

# Musik - Music Streaming Website

Welcome to **Musik**, a music streaming platform built from scratch! This is my first project using **ASP.NET Core**, cloud data, and domain name integration. I hope you'll enjoy exploring it as much as I enjoyed building it!

Musik allows users to stream music, manage playlists, and explore a variety of tracks hosted in the cloud. This project showcases my journey into modern web development and cloud technologies.

---

## Table of Contents
- [Demo](#demo)
- [Backend Swagger](#backend-swagger)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Setup Instructions](#setup-instructions)
- [Future Improvements](#future-improvements)
- [Contact](#contact)

---

## Demo
Check out the live demo of Musik here:  
[**Musik Demo**](coming soon!) 

---

## Backend Swagger
Explore the backend API documentation via Swagger:  
[**Backend Swagger**](https://54.66.143.213:5000/swagger/index.html)

The Swagger UI provides a detailed overview of all available endpoints, request/response models, and allows you to test the API directly.

---

## Tech Stack
Musik is built using a combination of modern technologies and cloud services:

### Backend
- **ASP.NET Core Web API**: RESTful API for handling music streaming, user authentication, and data management.
- **OAuth2**: Secure authentication and authorization for user access.
- **MS SQL Server**: Relational database for storing user data, playlists, and track metadata (initially hosted on Azure, now migrated).

### Cloud Infrastructure
- **Azure (Expired)**:
  - **Azure Blobs**: Used for storing audio files in the cloud (initial setup).
  - **Azure VM**: Hosted the backend API and database (initial deployment).
- **AWS (Current)**:
  - **AWS RDS**: Managed MS SQL Server instance for scalable and reliable database hosting.
  - **AWS EC2**: Virtual machine running the ASP.NET Core backend API.

### Frontend
-  HTML, CSS, JavaScript, VueJS

---

## Features
- Stream music from a cloud-hosted library.
- User authentication via OAuth2 for secure login.
- Create and manage personalized playlists.
- Responsive design for desktop and mobile in-build browser.
- API-driven architecture with Swagger documentation.

---


## Future Improvements
- Migrate audio storage to AWS EC2 server with Python (replacing expired Azure Blobs).
- Add charts algorithm soon.
- Implement a modern frontend framework (ReactJS and VueJS) for a better UI/UX.
- Optimize database queries for faster load times.
- Add support for multiple music formats and quality options.
- Add VIP function to listen to music without ads.

---

## Contact
Feel free to reach out if you have questions, suggestions, or just want to chat about Musik!  
- **Email**: trinmse183033@gmail.com  
- **GitHub**: [minhtris-peternguyxn](https://github.com/minhtris-peternguyxn)

---

### Notes
- **Azure Expiration**: The project initially used Azure services (Blobs and VM), but they’ve expired. It’s now fully migrated to AWS (RDS and EC2).
- **Domain Name**: In Maintance, will update soon!
