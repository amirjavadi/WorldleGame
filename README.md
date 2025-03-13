# Wordle Game

A modern implementation of the popular Wordle game built with .NET Core 8 and Nuxt.js.

## Features

- Beautiful, responsive UI with animations
- User authentication and profiles
- Daily word challenges
- Game history and statistics
- Global leaderboard
- Social sharing
- Admin panel for word management

## Prerequisites

- .NET Core 8 SDK
- Node.js (v16 or later)
- SQL Server
- Redis (optional, for caching)

## Project Structure

```
WordleGame/
├── WordleBackend/          # .NET Core 8 API
└── WordleFrontend/         # Nuxt.js frontend
```

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd WordleBackend
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Update the connection string in `appsettings.json`

4. Run database migrations:
   ```bash
   dotnet ef database update
   ```

5. Start the API:
   ```bash
   dotnet run
   ```

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd WordleFrontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm run dev
   ```

4. Open your browser and navigate to `http://localhost:3000`

## API Endpoints

### Authentication
- POST `/api/auth/register` - Register a new user
- POST `/api/auth/login` - Login user
- POST `/api/auth/logout` - Logout user

### Game
- GET `/api/game/daily-word` - Get the daily word
- POST `/api/game/guess` - Submit a guess
- GET `/api/game/history` - Get user's game history
- GET `/api/game/leaderboard` - Get global leaderboard

### Admin
- GET `/api/admin/words` - Get all words
- POST `/api/admin/words` - Add a new word
- PUT `/api/admin/words/{id}` - Update a word
- DELETE `/api/admin/words/{id}` - Delete a word

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Inspired by the original Wordle game
- Built with .NET Core 8 and Nuxt.js
- UI components from Vuetify 