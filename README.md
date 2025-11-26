# 🌍 Global News Aggregation Platform

A full-stack application built with C# (.NET), Angular, and MS SQL Server designed for collecting, organizing, summarizing, translating, and serving news articles from various RSS sources worldwide.

---

## 🚀 Overview

This platform automatically gathers news from multiple global RSS feeds, processes them using OpenAI, and presents them in a structured and user-friendly format.  
It is optimized for performance, automation, and efficient data storage.

---

## ✨ Key Features

- Automated RSS crawling every 6 hours  
- AI-powered summarization, categorization, and translation  
- Database cleanup job every 2 days  
- Full-stack architecture (.NET backend + Angular frontend + MSSQL database)  
- OpenAI integration for content enhancement  

---

## 📐 Architecture

### Backend (C# / .NET)
- REST API for managing articles, sections, summaries, and translations  
- Background jobs for RSS crawling, AI processing, and data cleanup  
- Entity Framework Core for database access  

### Frontend (Angular)
- Responsive interface for viewing categorized and summarized news  
- Integration with the backend API  
- Structured sections, summaries, translations, and links  

### Database (MS SQL Server)
- Tables for articles, categories, feeds, summaries, translations  
- Indexed for fast querying and high performance  

---

## ⚙️ Scheduled Jobs

RSS Aggregator (every 6 hours): Fetches articles from all RSS sources  
AI Processor (after fetching): Summaries, categorizes, and translates articles  
Data Cleanup (every 2 days): Removes old articles  

---

## 🧠 AI Integration (OpenAI)

Used for:  
- Short summaries  
- Category assignment  
- Translation to selected languages  

Ensures that all presented content is clean, structured, and user-friendly.

---

## 📦 Installation & Setup

### Prerequisites
- .NET SDK  
- Node.js  
- Angular CLI  
- MS SQL Server  
- OpenAI API Key  

---

## 🔧 Backend Setup

Run the following commands:

1) Navigate to backend folder  
   cd backend  

2) Restore packages  
   dotnet restore  

3) Apply migrations  
   dotnet ef database update  

4) Start the backend  
   dotnet run  

---

## 🎨 Frontend Setup

### Install Dependencies
Navigate to frontend folder:  
   cd frontend  
Install packages:  
   npm install  

### Run Development Server
   ng serve  

### Build for Production
   ng build  

---

## 👨‍💻 Author
Created by **Yordan Dimitrov**.
