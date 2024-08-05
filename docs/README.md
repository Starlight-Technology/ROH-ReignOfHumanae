# Reign of Humanæ (ROH)

# Introduction

Reign of Humanæ (ROH) is an MMORPG that immerses players in a world where magic and technology coexist. The game world began with a single intelligent race, the Humanae, which eventually divided into four distinct races: Humans, Elves, Dwarfs, and Demons. Each race possesses unique characteristics and abilities that shape their interactions and roles within the world.

# Races and Their Abilities
## Humans

Adaptability and large communities.
Learn new skills 15% faster.
10% resistance to cold.

## Elves

Reclusive, living in small forest communities.
Deep connection with nature.
Learn nature magic and agility skills 20% faster.
Forest creatures do not attack unless provoked.

## Dwarfs

Reside in caverns, advanced in technology.
Create items 25% more powerful.
10% faster at mining.
Learn strength skills 20% faster but find magic 20% harder to learn.

## Demons

Prefer warm climates, disdain other races.
Learn fire magic 25% faster.
50% resistance to fire.
Find agility and strength skills 20% harder to learn.
As the races interact, alliances and rivalries form, leading to a dynamic world filled with adventure and conflict.

# Features
Unique race-specific abilities and skills.
Complex interactions and alliances between races.
Rich lore and backstory.
Integration of magic and technology.


# Getting Started

## Project Structure
This repository is part of a larger project hosted in a free organization on GitHub, allowing contributions and collaborations. The project includes:

Blazor Application: Front-end interface built with Blazor.
Gateway: A central API gateway.
Microservices: Multiple back-end microservices handling different aspects of the game.
Database: PostgreSQL database.
Unity3D: Game engine used for creating the game.
Technologies Used
Blazor
ASP.NET Core
PostgreSQL
Docker
Unity3D

## Prerequisites
Docker and Docker Compose
.NET SDK
Unity3D

## Running the Project
You can run every API's, Blazor and Gateway with .net, and unity with unity engine 3D (last version), if you want to run everything (but not unity) on docker, it has a yml file on docker folder. Remember to configure the DB connection string, it as configured to run on postgreSQL and will search for connection string on environment variable with key= ROH_DATABASE_CONNECTION_STRING, ex: ROH_DATABASE_CONNECTION_STRING=Host=localhost;Port=5432;Database=ROH;Username=postgres;Password=postgres123; 

## Contributing
We welcome contributions from the community! To contribute, please follow these steps:

Fork the repository.
Create a new branch (git checkout -b feature/YourFeature).
Commit your changes (git commit -m 'Add some feature').
Push to the branch (git push origin feature/YourFeature).
Open a Pull Request.

# License
This project is licensed under the MIT License - see the LICENSE file for details.

# Contact
If you have any questions, feel free to reach out to us at starlighttecnologia@hotmail.com.
Found any bug? Fell free to open a issue.


![ArchtectureDiagram](https://github.com/RodrigoMartinsMoraes-Z/ROH-ReignOfHumanae/assets/28880737/db5055d2-56c7-4e10-b819-f5c6a0770c5f)

V1.0
![ROH - Diagram1](https://github.com/RodrigoMartinsMoraes-Z/ROH-ReignOfHumanae/assets/28880737/2d082135-6c2e-4265-a93f-a8b7b4519f8d)
