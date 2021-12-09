## Certify SSL

1. Clean cert if has before

   > `dotnet dev-certs https --clean`

2. Trust cert
   > `dotnet dev-certs https --trust`

---

## Setup Host

> ### Program.cs

Host (IHost) object:

- Dependency Injection (ID): IServiceProvider (ServiceCollection)
- Logging (ILogging)
- Configuration
- IHostedService => StartAsync : Run HTTP Server (Kestrel Http)

Steps:

1.  Create IHostBuilder
2.  Configure, Register Services (ConfigureWebHostDefaults)
3.  IHostBuilder.Build() => Host(IHost)
4.  Host.Run();

> ### Startup.cs

1. Register Services (using Dependency Injection)
2. Build pipeline (middlewares)

---

## Setup Webpack in ASP.NET

1. Install package

   > npm init -y                                                                      # tạo file package.json cho dự án\
   > npm i -D webpack webpack-cli                                      # cài đặt Webpack\
   > npm i node-sass postcss-loader postcss-preset-env     # cài đặt các gói để làm việc với SCSS\
   > npm i sass-loader css-loader cssnano                           # cài đặt các gói để làm việc với SCSS, CSS\
   > npm i mini-css-extract-plugin cross-env file-loader      # cài đặt các gói để làm việc với SCSS\
   > npm install copy-webpack-plugin                                 # cài đặt plugin copy file cho Webpack\
   > npm install npm-watch                                                  # package giám sát file thay đổi
   >
   > npm install bootstrap                              # cài đặt thư viện bootstrap\
   > npm install jquery                                   # cài đặt Jquery\
   > npm install popper.js                              # thư viện cần cho bootstrap

2. Create `src/scss/site.scss`

3. Create `webpack.config.js`

4. Add in `package.json`

   > "watch": {\
   >    "build": "src/scss/site.scss"\
   > },\
   > "scripts": { \
   >    "build": "webpack",\
   >    "watch": "npm-watch"\
   > },

5. Run Webpack

- Create files into public folder:

  > `npm run build`

- Automatically build when site.scss change:
  > `npm run watch`

--

## Store Session in SQL Cache

1. Add package dotnet-sql-cache

   > `dotnet tool install --global dotnet-sql-cache --version 5.0.0`

2. Create table to store session
   > `dotnet sql-cache create "Server=TUONG\SQLEXPRESS;Database=webdb;Trusted_Connection=True;" dbo Session`

Ref: https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0#distributed-sql-server-cache
