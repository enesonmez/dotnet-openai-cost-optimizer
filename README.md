# 🚀 OpenAI Cost & Performance Optimizer (.NET + Redis)

#### Dil => [Turkish](README.md) | [English](README.en.md)

Bu proje, bir .NET Core Web API projesinde OpenAI API maliyetlerini düşürmek ve yanıt sürelerini milisaniyeler seviyesine çekmek için geliştirilmiş bir Redis Caching mimarisi örneğidir.

💸 **Sorun:** Her benzer prompt için OpenAI'a gitmek gereksiz token maliyeti ve yüksek gecikme (latency) yaratır.

⚡ **Çözüm:** Decorator Pattern kullanarak OpenAI servisinin önüne bir Redis bariyeri koymak.

---

### 🛠️ Teknik Özellikler
* Framework: .NET 9.0 (veya 8.0)
* Caching: Redis (IDistributedCache)
* Architecture: Decorator Pattern & MVC
* Performance: SHA256 Hashing ile optimize edilmiş cache key yönetimi.
* Efficiency: Tekrarlanan sorgularda %100 maliyet tasarrufu ve <10ms yanıt süresi.

---

### 🏗️ Mimari Akış
Sistem, gelen prompt'u doğrudan OpenAI'a göndermek yerine şu adımları takip eder:

1. Hashing: Uzun prompt metni SHA256 ile hash'lenerek kısa bir cacheKey oluşturulur.
2. Cache Check: Redis'te bu key var mı bakılır.
3. Cache Hit (🟢): Veri varsa doğrudan Redis'ten döner. API maliyeti sıfırdır.
4. Cache Miss (🔴): Veri yoksa OpenAI'a gidilir, sonuç alınır ve belirlenen TTL (Time To Live) süresiyle Redis'e yazılır.

---

### 🚀 Hızlı Başlangıç
1. Gereksinimler
   * Docker (Redis için)
   * .NET SDK
   * OpenAI API Key


2. Redis'i Ayağa Kaldır
   ```bash 
   docker run -d --name redis-cache -p 6379:6379 redis 
   ```
3. Projeyi Çalıştır
   ```bash
   git clone https://github.com/kullaniciadi/AiPerformanceDemo.git
   cd AiPerformanceDemo
   dotnet run
   ```
Veya docker-compose ile hızlı şekilde projeyi up edebilirsiniz.
   ```bash
  docker-compose up -d
  ```
---

### 💻 Kullanım Örneği
**Endpoint:** POST /api/ai/ask

**Request Body:**
```json
{
  "prompt": "C# Decorator Pattern nedir, neden kullanılır?"
}
```
**Response (X-Cache Header: Hit/Miss):**

```json
{
  "result": "[OpenAI Yanıtı] Decorator pattern, bir nesneye dinamik olarak...",
  "elapsedTime": "12ms"
}
```
---
### 📐 Tasarım Deseni: Decorator
Bu projede Decorator Pattern kullanılarak OpenAiService sınıfının koduna dokunmadan ona cache özelliği kazandırılmıştır. Bu sayede Single Responsibility (SRP) ve Open/Closed (OCP) prensiplerine tam uyum sağlanmıştır.

```C#
// Program.cs içerisindeki kayıt mantığı
builder.Services.AddScoped<IOpenAiService>(provider => {
   var baseService = new OpenAiService();
   var cache = provider.GetRequiredService<IDistributedCache>();
   return new CachedOpenAiService(baseService, cache);
});
```
---
### 📝 Lisans
Bu proje MIT lisansı altındadır.

Not: Bu proje Twitter ve Linkedin'deki flood kapsamında hazırlanan bir demo uygulamasıdır. Katkılarınızı bekliyorum! ✨