# PLEASE NOTE
You must add your DISCORD_TOKEN in your local environment to run this. Run this on powershell:
```bash
$Env:DISCORD_TOKEN="your-token-here"
```

Or go to your Environment Variables and add your DISCORD_TOKEN.

# Project Overview

This project was built as part of a take-home technical assessment to demonstrate my skills across C#, clean architecture, React, and testing.

A discord bot application that will process commands and respond to certain commands. A Frontend to see list of users where you can block and unblock them. A API to handle requests and interact with the database.

This was built to demonstrate clean code and architecture.  Wherever possible, code is loosely coupled to ensure ease of testing, maintainability and flexibility.

You will need to run all 3 entry points:
 - DiscordBotSolution.BotApi
 - DiscordBotSolution.BotApp
 - DiscordBotSolution.BotWorker

# Architecture & Design
## Layers / Structure
It has been structurally laid out architecturally for separation

 - DiscordBotSolution.BotApi - API routes to all endpoints
 - DiscordBotSolution.BotApi.Core - API business logic to handle all endpoint. Has layers (interface, models, dtos, repository, services)
 - DiscordBotSolution.BotWorker - Console app entry point to start the discord application 
 - DiscordBotSolution.BotWorker.Core - Discord business logic to connect, receive and respond to messages. Has layers (interfaces, dtos, services)
 - DiscordBotSolution.BotApp - Frontend built in react. Has layers (components, hooks, services, models)
 - DiscordBotSolution.Tests - Tests the 2 main API and worker areas 


## Design Choices
DiscordBotSolution.BotApi.Core
 - BotService - Used to call the respositories and accept DTO and transform into models.
 - UserRepository and UserTimerRepository - Calls dbcontext to call the database

DiscordBotSolution.BotWorker.Core
 - DiscordMessageService - Logic where it will accept the messages and send responses
 - DiscordBotService - Entry point to connect the discord server
 - TimerService - Handles timer logic to respond after a set time
 - ApiClient - Interacts with API to create and get users and store user timers

DiscordBotSolution.BotApp
 - BotApiService - Interacts with API to get login and get and block/unblock users
 - UserTable - Page to see list of all users, can refresh and block/unblock users
 - LoginForm - Initial basic Login form

# Algorithm
BotWorker

Will connect to the discord server using a token. This token is stored in your environment settings called DISCORD_TOKEN. 

The BotWorker continuously listens for messages from the server and process the message.

This message will check for the following:
 - if the user doesn’t exist → creates the user
 - if the user does exists and is blocked → send a message to say they are blocked
 - if they are not blocked and if the message is PING → respond with PONG
 - if they are not blocked and if the message is TIMER {number}.
   - It will respond saying it has started
   - Create a UserTimer entry
   - Start a basic time delay of that number
   - On completed, it will update the UserTimer
   - Respond with completed message

# Testing
Tests are under DiscordBotSolution.Tests

There are a mixture of unit test for specific test on services and functionality. 

## Testing Covers:

Functional
- BotWorker messages
- BotWorker timer
- API endpoints

Non Functional
- Exceptions

# Tech Stack
 - .NET 8
 - React + TypeScript
 - SQLite + EF Core
 - xUnit + Moq
 - Swashbuckle for Swagger

# How to Run
Open the solution in Visual Studio.

Set the startup projects:
 - Right-click the solution → Set Startup Projects…
 - Choose Multiple startup projects
 - Set the Action for each project:
   - DiscordBotSolution.BotApi → Start
   - DiscordBotSolution.BotWorker → Start
   - DiscordBotSolution.BotApp → Start (this will run the React frontend)

Run the solution
 - Press F5 or click Start
 - This will launch the API, Worker, and React frontend all at once

Make sure you’ve run npm install once inside the BotApp folder to install dependencies. 

# Future Improvements
 - Smarter logging.
 - Additional validation and exception handling
 - Refactor architecturally into separate cleaner code (application/domain/infrastructure/presentation layers)
 - Add login authentication with authorisation with session handling for frontend and API endpoints
 - Upgrade console app into a worker app 

# Key Learnings
 - Cleaner architecture applied to the problems.
 - Discord bot problem solving in C#.
 - Balancing simplicity with extensibility.
