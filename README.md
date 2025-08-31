# 📞 Agenda de Contatos

> Teste técnico desenvolvido para a **Blue Technology**

Uma aplicação completa de agenda de contatos com frontend em Vue.js e backend em .NET, containerizada com Docker para facilitar a execução e deploy.

## 🚀 Tecnologias Utilizadas

### Frontend
- **Vue.js** - Framework JavaScript progressivo
- **Vite** - Build tool e dev server
- **Docker** - Containerização

### Backend
- **.NET** - Framework de desenvolvimento
- **ASP.NET Core** - API REST
- **Docker** - Containerização

### Banco de Dados
- **Azure SQL Database** - Banco de dados hospedado na nuvem Azure

## 📋 Funcionalidades

- ✅ Cadastro de contatos
- ✅ Listagem de contatos
- ✅ Edição de contatos
- ✅ Exclusão de contatos
- ✅ Interface responsiva
- ✅ API REST completa
- ✅ Deploy em containers Docker
  

## 🐳 Executando com Docker

### Backend (.NET API)

1. Navegue para o diretório do backend:
```bash
cd BLUE-AgendaDeContatoss/BLUE\ -\ AgendaAPI
```

2. Construa a imagem Docker:
```bash
docker build -f "BLUE - AgendaAPI/Dockerfile" -t agenda-api .
```

3. Execute o container:
```bash
docker run -d -p 8080:8080 -e "STR_CONEXAO=sua_string_de_conexao" --name agenda-container agenda-api
```

4. A API estará disponível em: `http://localhost:8080`

### Frontend (Vue.js)

1. Navegue para o diretório do frontend:
```bash
cd BLUE-AgendaDeContatoss/BLUE\ -\ AgendaWEB
```

2. Construa a imagem Docker:
```bash
docker build --build-arg VITE_API_BASE_URL=http://localhost:8080 -t agendaweb-container .
```

3. Execute o container:
```bash
docker run -d --name agendaweb-frontend -p 3000:80 agendaweb-container
```

4. A aplicação estará disponível em: `http://localhost:3000`

## ⚙️ Variáveis de Ambiente

### Backend
- `STR_CONEXAO`: String de conexão com o banco de dados Azure

### Frontend
- `VITE_API_BASE_URL`: URL base da API backend (exemplo: `http://localhost:8080`)

## 🗄️ Banco de Dados

O projeto utiliza uma instância do **Azure SQL Database** hospedada na nuvem da Microsoft Azure, garantindo:

- 🔒 Segurança e confiabilidade
- 📈 Escalabilidade automática
- 🌐 Acesso global
- 🔄 Backup automático

## 📁 Estrutura do Projeto

```
BLUE-AgendaDeContatoss/
├── BLUE - AgendaAPI/Agenda.API          # Backend .NET
│   ├── Dockerfile
│   └── ...
├── BLUE - AgendaWEB/          # Frontend Vue.js
│   ├── Dockerfile
│   └── ...
└── README.md
```

## 🚀 Deploy e Produção

O projeto está preparado para deploy em ambientes de produção utilizando:

- **Containers Docker** para facilitar o deploy
- **Azure SQL Database** para persistência de dados
- **Variáveis de ambiente** para configuração flexível

## 🤝 Contribuindo

Este projeto foi desenvolvido como teste técnico para a Blue Technology. Para contribuições:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📞 Contato

Desenvolvido para **Blue Technology**

---

## 👨‍💻 Desenvolvedor
Joao Victor Carvalho - joaovictorcordeiro00@gmail.com
