# 🌍 Global News Aggregation Platform

A full-stack application built with **C# (.NET)**, **Angular**, and **MS SQL Server** designed for collecting, 
organizing, summarizing, translating, and serving news articles from various RSS sources worldwide.

---

## 🚀 Overview

This platform automatically gathers news from multiple global RSS feeds, processes them using OpenAI, and presents 
them in a structured and user-friendly format. It is optimized for performance, automation, and efficient data storage.

---

## ✨ Key Features

### 🔹 Automated News Collection
- A background job runs **every 6 hours**.
- Fetches news from a list of predefined **RSS sources** around the world.
- Each article is saved in the database before processing.

### 🔹 AI-Powered News Enhancements
After fetching, each article is sent to **OpenAI**, which performs:
- **Summarization** – Generates a short, clean summary.
- **Categorization** – Assigns the article to the correct section/category.
- **Translation** – Produces a translated version for multilingual display.

### 🔹 Data Lifecycle Management
- A separate cleanup job runs **every 2 days**.
- Deletes outdated articles to keep the database clean and lightweight.

### 🔹 Modern Technology Stack
- **Backend:** C# (.NET)
- **Frontend:** Angular
- **Database:** Microsoft SQL Server
- **AI Integration:** OpenAI API
- **Task Scheduling:** Hosted background services / cron-style jobs

---

## 📐 Architecture

### Backend (C# / .NET)
- API endpoints for managing articles, sections, translations, users, and scheduled tasks.
- Background services for:
  - RSS crawling
  - OpenAI processing
  - Database cleanup
- Entity Framework Core for database access.

### Frontend (Angular)
- Responsive UI for displaying categorized news.
- Sections, summaries, translations, and links to original articles.
- API integration with the .NET backend.

### Database (MS SQL Server)
- Optimized relational schema:
  - Articles  
  - Categories  
  - Source feeds  
  - Summaries  
  - Translations  
- Includes indexing for fast querying of large sets.

---

## ⚙️ Scheduled Jobs

| Job | Frequency | Description |
|-----|-----------|-------------|
| **RSS Aggregator** | Every 6 hours | Fetches articles from all sources and stores raw data. |
| **AI Processor** | Immediately after fetching | Sends content to OpenAI for summary, translation, categorization. |
| **Data Cleanup** | Every 2 days | Removes old news to maintain database efficiency. |

---

## 🧠 AI Integration (OpenAI)
The platform uses OpenAI to:
- Create short, readable article summaries.
- Automatically classify each article into categories.
- Translate text for multi-language support.

This ensures users see high-quality, structured, and accurate content.

---

## 📦 Installation & Setup

### Prerequisites
- .NET SDK  
- Node.js + Angular CLI  
- MS SQL Server  
- OpenAI API Key
  
### Backend Setup
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
