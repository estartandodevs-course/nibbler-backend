# 📔 Projeto Nibbler

## 🌟 Visão Geral
Sistema de diário pessoal digital com gerenciamento de tarefas, desenvolvido em .NET 8.0, seguindo princípios de Clean Architecture e DDD.

## 🚀 Funcionalidades Principais

### 👤 Usuários
- [x] Cadastro e autenticação
- [x] Perfil personalizado
- [x] Upload de fotos

### 📝 Diário (Almost Finished) - To Do
- [x] Criação e edição de entradas
- [x] Sistema de reflexões - WIP
- [x] Categorização por etiquetas
- [x] Soft delete

### ✅ Tarefas (WIP) - To do
- [ ] Gerenciamento de tarefas e subtarefas
- [ ] Categorização
- [ ] Status de conclusão
- [ ] Ordenação personalizada

## 🛠️ Tecnologias

- `.NET 8.0`
- `Entity Framework Core 8.0`
- `SQL Server`
- `Swagger`
- `FluentValidation`
- `Dapper`
- `MediatR (CQRS)`

## 📁 Estrutura do Projeto

```
src/
├── 🧱 BuildingBlocks/
│   ├── Nibbler.Core/
│   └── Nibbler.WebAPI.Core/
├── 📔 Diario/
│   ├── Nibbler.Diario.App/
│   ├── Nibbler.Diario.Domain/
│   └── Nibbler.Diario.Infra/
├── 👤 Usuarios/
│   ├── Nibbler.Usuario.App/
│   ├── Nibbler.Usuario.Domain/
│   └── Nibbler.Usuario.Infra/
└── 🌐 WebAPI/
    └── Nibbler.WebAPI/
```

## ⚙️ Configuração

### Banco de Dados
Configure a string de conexão em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "NibblerConnection": "Server=localhost;Database=NibblerProject;User Id=sa;Password=SuaSenha;TrustServerCertificate=True"
  }
}
```

### Migrations
```bash
# Na pasta do projeto Infra
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📚 Documentação

A API é documentada usando Swagger. Para acessar:

1. Execute o projeto
2. Acesse: `http://localhost:[porta]/swagger`
## 🚥 Abordagens
- Domain-Driven Design (DDD)

### Padrões Utilizados
- Clean Architecture
- CQRS
- Repository Pattern
- Unit of Work
- Value Objects
- Domain Events

### Validações
Utilizamos FluentValidation para validar entradas. Exemplo:

```csharp
public class AdicionarDiarioValidation : AbstractValidator<AdicionarDiarioCommand>
{
    public AdicionarDiarioValidation()
    {
        RuleFor(c => c.Titulo)
            .NotEmpty()
            .Length(3, 150);

        RuleFor(c => c.Conteudo)
            .NotEmpty()
            .Length(10, 5000);
    }
}
```

## 🤝 Como Contribuir

1. Fork o repositório
2. Crie uma branch: `git checkout -b feature/nova-funcionalidade`
3. Commit suas alterações: `git commit -m 'Adiciona nova funcionalidade'`
4. Push para a branch: `git push origin feature/nova-funcionalidade`
5. Abra um Pull Request

## 📋 Requisitos

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 ou VS Code

## 🚀 Executando o Projeto

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/nibbler.git

# Entre na pasta
cd nibbler

# Restaure os pacotes
dotnet restore

# Execute
dotnet run --project src/WebAPI/Nibbler.WebAPI
```


## 📝 Nota

- Projeto em desenvolvimento ativo. Novas funcionalidades são adicionadas regularmente.
  
<div align="center">
Made with ❤️ by Estartando Devs
</div>
