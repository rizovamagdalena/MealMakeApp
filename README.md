# MealMakeApp 🍽️

MealMakeApp is a mini full-stack ASP.NET Core web application built using **Onion Architecture**.  
It helps users organize meals into collections, automatically generate ingredient summaries, and manage active plans or archive them plans.

The app integrates external APIs for meal data and AI-powered ingredient summaries.

---

## ✨ Features

### 🔍 Meal Search
- Search meals using **TheMealDB public API**
- View meal details and ingredients

### 📂 Collections & Categories
- Create custom **categories**
- Group meals into **collections**
- Add meals to collections by searching from TheMealDB
- Each user can have **one active collection at a time**

### 🏠 Home Dashboard
For the active collection, the home page shows:
- Collection name
- All meals in the collection
- **Ingredient summary** generated from all meals

### 🤖 AI Ingredient Summary
- Ingredients from all meals in a collection are combined
- Sent to the **OpenAI API** with a custom prompt
- Returns a summarized shopping list with **total measurements**

### 🗄️ Archiving Collections
- Collections can be archived
- Archived collections are **read-only**
- Users can still view:
  - Collection name
  - Meals included

### 👤 User-Based Data
- Each user manages their own collections
- Data is isolated per user

---

## 🧱 Architecture

The solution follows **Onion Architecture** principles.

