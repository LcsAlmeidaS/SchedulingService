# SchedulingService

Uma API RESTful desenvolvida com **ASP.NET Core** para gerenciamento de agendamentos, com suporte a cadastro de clientes, funcionários, serviços oferecidos e consulta de disponibilidade em tempo real. Construída seguindo os princípios de **Clean Architecture**, com separação em camadas de Domain, Application, Infrastructure e API.

---

## 📋 Índice

- [Funcionalidades](#funcionalidades)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Referência da API](#referência-da-api)
- [Tratamento de Erros](#tratamento-de-erros)
- [Configuração](#configuração)

---

## ✨ Funcionalidades

- Cadastro e gerenciamento de clientes
- Cadastro e gerenciamento de funcionários com horários de trabalho e intervalos semanais
- Cadastro e gerenciamento de serviços oferecidos (nome, preço, duração)
- Criação de agendamentos com validação de conflito de horário por funcionário
- Consulta de horários disponíveis por serviço, data e funcionário
- Transições de status de agendamento: `Pending → Confirmed → Completed / Cancelled / NoShow`
- Métricas de funcionários: horas trabalhadas concluídas e horas agendadas por período
- Middleware global de tratamento de exceções com respostas padronizadas
- Swagger UI disponível no ambiente de desenvolvimento

---

## 🛠 Tecnologias

| Tecnologia | Versão |
|---|---|
| .NET | 10.0 |
| ASP.NET Core | 10.0 |
| Entity Framework Core | 10.0.7 |
| Npgsql (PostgreSQL) | 10.0.1 |
| Swashbuckle (Swagger) | 10.1.7 |

---

## 🏛 Arquitetura

O projeto segue os princípios de **Clean Architecture**, organizado em 4 camadas com dependências unidirecionais:

```
Scheduling.API → Scheduling.Application → Scheduling.Domain
Scheduling.Infrastructure → Scheduling.Application → Scheduling.Domain
```

| Camada | Responsabilidade |
|---|---|
| **Domain** | Entidades de negócio e suas regras. Sem dependências externas. |
| **Application** | Casos de uso, DTOs, interfaces de repositórios e serviços. Depende apenas de Domain. |
| **Infrastructure** | Implementações de repositórios, DbContext e migrations (EF Core + PostgreSQL). |
| **API** | Controllers, middleware, configuração de DI e bootstrap da aplicação. |

---

## 📁 Estrutura do Projeto

```
SchedulingBackend/
├── Scheduling.Domain/                           # Entidades e regras de domínio
│   └── Entities/
│       ├── Appointment.cs                       # Agendamento com máquina de estados
│       ├── BreakTime.cs                         # Intervalo semanal de um funcionário
│       ├── Customer.cs                          # Cliente
│       ├── ServiceOffering.cs                   # Serviço oferecido
│       ├── Staff.cs                             # Funcionário com horários e intervalos
│       └── WorkingHours.cs                      # Horário de trabalho semanal
│
├── Scheduling.Application/                      # Casos de uso e contratos
│   ├── DTOs/
│   │   ├── Appointment/                         # DTOs de criação e resposta de agendamento
│   │   ├── Availability/                        # DTOs de consulta e slot disponível
│   │   ├── Customer/                            # DTOs de criação, atualização e resposta
│   │   ├── Mappings/                            # Extension methods de mapeamento entidade → DTO
│   │   ├── ServiceOffering/                     # DTOs de criação, atualização e resposta
│   │   └── Staff/                               # DTOs de funcionário, horários e métricas
│   ├── Repositories/                            # Interfaces dos repositórios (contratos)
│   │   ├── IRepository.cs                       # Repositório genérico base
│   │   ├── IAppointmentRepository.cs
│   │   ├── ICustomerRepository.cs
│   │   ├── IServiceOfferingRepository.cs
│   │   └── IStaffRepository.cs
│   └── Services/
│       ├── Interfaces/                          # Contratos dos serviços de aplicação
│       │   ├── IAppointmentService.cs
│       │   ├── IAvailabilityService.cs
│       │   ├── ICustomerService.cs
│       │   ├── IServiceOfferingService.cs
│       │   └── IStaffService.cs
│       ├── AppointmentService.cs                # Criação e transições de status
│       ├── AvailabilityService.cs               # Cálculo de slots disponíveis
│       ├── CustomerService.cs
│       ├── ServiceOfferingService.cs
│       └── StaffService.cs                      # Inclui cálculo de métricas por período
│
├── Scheduling.Infrastructure/                   # Persistência e acesso a dados
│   ├── Context/
│   │   └── AppDbContext.cs                      # DbContext com configurações do modelo
│   ├── Migrations/                              # Migrations do EF Core
│   └── Repositories/                            # Implementações dos repositórios
│       ├── Repository.cs                        # Implementação genérica base
│       ├── AppointmentRepository.cs
│       ├── CustomerRepository.cs
│       ├── ServiceOfferingRepository.cs
│       └── StaffRepository.cs
│
└── Scheduling.API/                              # Camada de apresentação
    ├── Controllers/
    │   ├── AppointmentsController.cs
    │   ├── AvailabilityController.cs
    │   ├── CustomersController.cs
    │   ├── ServiceOfferingsController.cs
    │   └── StaffController.cs
    ├── Infrastructure/
    │   ├── Json/
    │   │   └── TimeOnlyJsonConverter.cs         # Serialização customizada de TimeOnly
    │   └── Middleware/
    │       └── ExceptionMiddleware.cs           # Tratamento global de exceções
    ├── Program.cs                               # Bootstrap e configuração de DI
    └── appsettings.json                         # Connection string (vazia — usar User Secrets)
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### 1. Clone o repositório

```bash
git clone https://github.com/LcsAlmeidaS/SchedulingService.git
cd SchedulingService/SchedulingBackend
```

### 2. Configure a connection string via User Secrets

A connection string **não está no repositório** por segurança. Use o mecanismo de User Secrets do .NET:

**Mac/Linux**
```bash
cd Scheduling.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=scheduling;Username=seu_usuario;Password=sua_senha"
```

**Windows (PowerShell)**
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
cd Scheduling.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=scheduling;Username=seu_usuario;Password=sua_senha"
```

### 3. Aplique as migrations

```bash
# A partir da pasta SchedulingBackend
dotnet ef database update --project Scheduling.Infrastructure --startup-project Scheduling.API
```

### 4. Execute a API

```bash
cd Scheduling.API
dotnet run
```

A API estará disponível na porta exibida no terminal. O Swagger UI estará acessível em `http://localhost:<porta>/swagger` no ambiente de desenvolvimento.

---

## 📡 Referência da API

### Customers

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/customers` | Lista todos os clientes |
| `GET` | `/api/customers/{id}` | Busca cliente por ID |
| `GET` | `/api/customers/by-email?email=` | Busca cliente por e-mail |
| `POST` | `/api/customers` | Cria um novo cliente |
| `PATCH` | `/api/customers/{id}` | Atualiza e-mail e telefone do cliente |

### Staff

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/staff/active` | Lista todos os funcionários ativos |
| `GET` | `/api/staff/{id}` | Busca funcionário por ID |
| `POST` | `/api/staff` | Cria um novo funcionário |
| `PATCH` | `/api/staff/{id}` | Atualiza nome, e-mail e telefone |
| `DELETE` | `/api/staff/{id}` | Desativa o funcionário |
| `POST` | `/api/staff/{id}/working-hours` | Adiciona horário de trabalho semanal |
| `POST` | `/api/staff/{id}/break-times` | Adiciona intervalo semanal |
| `GET` | `/api/staff/{id}/appointment-summary?from=&to=` | Resumo de agendamentos concluídos no período |
| `GET` | `/api/staff/{id}/scheduled-hours?from=&to=` | Total de horas agendadas no período |

### Service Offerings

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/serviceofferings` | Lista todos os serviços ativos |
| `GET` | `/api/serviceofferings/{id}` | Busca serviço por ID |
| `POST` | `/api/serviceofferings` | Cria um novo serviço |
| `PUT` | `/api/serviceofferings/{id}` | Atualiza os dados do serviço |
| `PATCH` | `/api/serviceofferings/{id}/activate` | Ativa o serviço |
| `PATCH` | `/api/serviceofferings/{id}/deactivate` | Desativa o serviço |

### Appointments

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/appointments/{id}` | Busca agendamento por ID |
| `GET` | `/api/appointments?customerId=` | Lista agendamentos de um cliente |
| `POST` | `/api/appointments` | Cria um novo agendamento |
| `PATCH` | `/api/appointments/{id}/confirm` | Confirma o agendamento |
| `PATCH` | `/api/appointments/{id}/cancel` | Cancela o agendamento |
| `PATCH` | `/api/appointments/{id}/complete` | Conclui o agendamento |
| `PATCH` | `/api/appointments/{id}/no-show` | Marca como não comparecimento |

### Availability

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/availability?serviceOfferingId=&date=&staffId=` | Retorna slots disponíveis para o serviço e data informados. `staffId` é opcional — sem ele retorna slots de todos os funcionários ativos. |

**Exemplo de resposta:**

```json
[
  {
    "staffId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "staffName": "João Silva",
    "startTime": "2026-05-15T09:00:00",
    "endTime": "2026-05-15T09:30:00"
  }
]
```

---

## ⚠️ Tratamento de Erros

Todas as exceções são interceptadas pelo `ExceptionMiddleware` e retornam respostas padronizadas em JSON:

| Status | Condição |
|---|---|
| `400 Bad Request` | Regra de negócio violada (conflito de horário, dados inválidos, etc.) |
| `404 Not Found` | Recurso não encontrado |
| `500 Internal Server Error` | Erro inesperado no servidor |

**Exemplo de resposta de erro:**

```json
{
  "status": 404,
  "message": "Staff 3fa85f64-5717-4562-b3fc-2c963f66afa6 not found."
}
```

---

## ⚙️ Configuração

A connection string é gerenciada via **User Secrets** e não deve ser commitada. O arquivo `appsettings.json` contém apenas o placeholder:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

Para criar novas migrations após alterações no modelo:

```bash
# A partir da pasta SchedulingBackend
dotnet ef migrations add <NomeDaMigration> --project Scheduling.Infrastructure --startup-project Scheduling.API
dotnet ef database update --project Scheduling.Infrastructure --startup-project Scheduling.API
```
