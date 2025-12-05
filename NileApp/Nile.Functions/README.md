# Nile Functions Project

## Overview
The Nile Functions project is an Azure Functions application built using the .NET 8 (Isolated) framework. This project includes an HTTP trigger function that handles incoming HTTP requests and is structured to support future expansion with models and services.

## Project Structure
```
Nile.Functions
├── Properties
│   └── launchSettings.json
├── Functions
│   └── HttpTriggerFunction.cs
├── Models
├── Services
├── host.json
├── local.settings.json
├── Program.cs
├── Nile.Functions.csproj
└── README.md
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- Azure Functions Core Tools
- An IDE or text editor (e.g., Visual Studio Code)

### Setup
1. Clone the repository or download the project files.
2. Open a terminal and navigate to the project directory.
3. Restore the project dependencies by running:
   ```
   dotnet restore
   ```

### Running the Project
To run the Azure Functions project locally, use the following command:
```
func start
```
This will start the Azure Functions runtime and host your functions locally.

### Deploying to Azure
To deploy the Azure Functions application to Azure, follow these steps:
1. Ensure you have an Azure account and have created a Function App in the Azure portal.
2. Use the Azure Functions Core Tools to deploy:
   ```
   func azure functionapp publish <YourFunctionAppName>
   ```

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.