# CETracker
Backend for the CAS CE Tracker

## Overview

REST-style API for managing CAS continuing education credits. Currently only supports US General and Specific Standards. 

This app is currently in development.

## Features

### CE Management
- Add/Update/Delete Experiences
- Tracks categories
- Calculates credit amounts and progress toward annual requirement

### User Management
- Manages specific user data, including title and associated National Standard

## Technology Stack

### Backend
- .NET Web API
- SQL Server

## Installation

### Clone the Repository

```bash
git clone https://github.com/example/ce-tracker.git
cd CETracker/CETrackerApi
```

### Install Dependencies 

```bash
dotnet restore
```

### Run

```bash
dotnet run --launch-profile "CETrackerAPI"
```

