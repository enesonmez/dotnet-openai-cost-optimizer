# 🚀 OpenAI Cost & Performance Optimizer (.NET + Redis)

#### Language => [Turkish](README.md) | [English](README.en.md)

This project demonstrates a Redis Caching architecture developed for a .NET Core Web API to reduce OpenAI API costs and bring response times down to milliseconds.

💸 **The Problem:** Sending every similar prompt to OpenAI results in unnecessary token costs and high latency.

⚡ **The Solution:** Placing a Redis barrier in front of the OpenAI service using the Decorator Pattern.

---

### 🛠️ Technical Features
* Framework: .NET 9.0 (or 8.0)
* Caching: Redis (IDistributedCache)
* Architecture: Decorator Pattern & MVC
* Performance: Optimized cache key management with SHA256 Hashing.
* Efficiency: 100% cost savings on repeated queries and <10ms response time.

---

### 🏗️ Architectural Flow
Instead of sending the incoming prompt directly to OpenAI, the system follows these steps:

1. Hashing: The long prompt text is hashed using SHA256 to create a short cacheKey.
2. Cache Check: It checks if this key exists in Redis.
3. Cache Hit (🟢): If the data exists, it returns directly from Redis. API cost is zero.
4. Cache Miss (🔴): If the data does not exist, the OpenAI API is called, the result is retrieved and written to Redis with a defined TTL (Time To Live).

---

### 🚀 Quick Start
1. Prerequisites
   * Docker (for Redis)
   * .NET SDK
   * OpenAI API Key


2. Run Redis
   ```bash 
   docker run -d --name redis-cache -p 6379:6379 redis 
   ```
3. Run the Project
   ```bash
   git clone https://github.com/kullaniciadi/AiPerformanceDemo.git
   cd AiPerformanceDemo
   dotnet run
   ```
Or you can quickly deploy the project using Docker Compose.
   ```bash
  docker-compose up -d
  ```

---
   
### 💻 Usage Example
**Endpoint:** POST /api/ai/ask

**Request Body:**
```json
{
    "prompt": "What is the Decorator Pattern in C#, and why is it used?"
}
```
Response (X-Cache Header: Hit/Miss):

```json
{
    "result": "[OpenAI Response] The decorator pattern dynamically adds...",
    "elapsedTime": "12ms"
}
```
---

### 📐 Design Pattern: Decorator
In this project, the Decorator Pattern is used to provide caching capabilities to the OpenAiService class without modifying its existing code. This ensures full compliance with the Single Responsibility (SRP) and Open/Closed (OCP) principles.

```C#
// Registration logic in Program.cs
builder.Services.AddScoped<IOpenAiService>(provider => {
   var baseService = new OpenAiService();
   var cache = provider.GetRequiredService<IDistributedCache>();
   return new CachedOpenAiService(baseService, cache);
});
```
---
### 📝 License
This project is licensed under the MIT License.