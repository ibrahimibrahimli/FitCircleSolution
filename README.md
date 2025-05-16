  # 🏋️‍♂️ FitCircle

**A next‑gen fitness & wellness subscription platform**  
Match with personal trainers, follow custom diet plans, browse partner gyms & stores, ask & answer questions—and more—all from one app.

---

## 🚀 Project Overview

FitCircle brings personal fitness & nutrition together in one mobile experience.  
- **Users** subscribe to plans, pick trainers, get diet & workout guidance.  
- **Trainers** earn subscription fees directly.  
- **Stores & Gyms** list products & locations.  
- **Community Q&A** & AI‑powered tips keep engagement high.

---

## 🎯 Key Features (MVP)

- **Authentication & Profiles** (email/social + JWT)  
- **Subscription Plans** (Flexible, Tiered, Hybrid)  
- **Stripe Payments** & Webhook handling  
- **Trainer Directory** + Ratings & Reviews  
- **Diet Plan Delivery** & tracking  
- **Q&A Forum** (questions, answers, upvote/downvote)  
- **Store Catalog** (partner products browsing)  
- **User Dashboard** (subscriptions, history, orders)  
- **Push & In‑App Notifications**  

---

## 🛠️ Tech & Architecture

- **Backend**: .NET 9 Web API (Clean Onion + CQRS + MediatR)  
- **Mobile**: Flutter (iOS & Android)  
- **Write Model**: Microsoft SQL Server  
- **Read Model**: MongoDB  
- **Caching**: Redis  
- **Messaging / Events**: RabbitMQ or Kafka  
- **Payments**: Stripe / Stripe Connect  
- **Auth**: JWT + OAuth (Google, Apple)  
- **Logging**: Serilog + Application Insights  
- **API Docs**: Swagger (OpenAPI)  
- **CI/CD**: GitHub Actions + SonarCloud  
- **Code Style**: SOLID, DRY, KISS, Clean Architecture  

---

## 🔧 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)  
- [Node.js & npm](https://nodejs.org/) (for any tooling)  
- [Docker & Docker Compose](https://www.docker.com/)  
- [Flutter SDK](https://flutter.dev/docs/get-started/install)  
- SQL Server instance or Docker container  
- MongoDB instance or Docker container  

---

### Local Setup

1. **Clone repos**  
   ```bash
   git clone https://github.com/ibrahimibrahimli/FitCircleSolution.git
   git clone https://github.com/ibrahimibrahimli/FitCircleMobile.git

   📡 API & Mobile
CQRS with MediatR:

Commands (writes) in FitCircle.Application.Commands

Queries (reads) in FitCircle.Application.Queries

Controllers in FitCircle.Api call mediator.Send(command/query)

Flutter: BLoC or Riverpod for state management, HTTP + Stripe SDK integration

💳 Stripe Payment Integration
Stripe Connect setup: platform collects subscription fees, splits payouts to trainers

PaymentIntent for one‑time & recurring payments

Webhooks at /api/payments/webhook to sync subscription status


✅ Testing
Unit Tests: xUnit + Moq for services & handlers

Integration Tests: Test server + in‑memory DB or test containers

Load Testing: k6 or JMeter for critical endpoints

Run locally: dotnet test ./FitCircle.Tests


🔁 CI / CD & Quality
GitHub Actions:

build-and-test.yml → builds, runs tests, code coverage

deploy.yml → Docker image push & deploy to Azure/AWS

SonarCloud: code smells, vulnerabilities

StyleCop & EditorConfig for consistent C# styling

Prettier for Flutter/Dart


