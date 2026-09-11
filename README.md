# Room / PG Booking System

Full-stack starter application:
- Backend: ASP.NET Core 8 Web API + Entity Framework Core + SQL Server
- Frontend: React + Vite
- Local database: SQL Server LocalDB
- Production: Azure App Service + Azure SQL
- Repository: GitHub

## Run backend
Open `backend/RoomPGBooking.API/RoomPGBooking.API.csproj` in Visual Studio.
Run the API. Swagger is available at `https://localhost:7085/swagger`.

## Run frontend
Open a terminal in `frontend/room-pg-booking`:
```bash
npm install
npm run dev
```
Before production, create `.env`:
```text
VITE_API_URL=https://YOUR-API-URL/api
```

## GitHub
Create an empty GitHub repository, then from this folder:
```bash
git init
git add .
git commit -m "Initial Room PG Booking System"
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/room-pg-booking.git
git push -u origin main
```

## Azure
1. Create Azure SQL Database and copy its ADO.NET connection string.
2. Create Azure App Service for the ASP.NET Core backend.
3. Set App Service > Environment variables > ConnectionStrings__DefaultConnection to the Azure SQL connection string.
4. Deploy backend from GitHub/Visual Studio.
5. Build React with `VITE_API_URL=https://YOUR-BACKEND.azurewebsites.net/api`.
6. Deploy the React `dist` output to a static hosting service or a second App Service configured for static files.
7. Add the frontend URL to CORS if you later tighten the API policy.

For a custom `www` domain, buy/use a domain and map it to the frontend hosting service, then enable HTTPS.
