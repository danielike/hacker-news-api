**How to run**
Download from the releases the executable:
 - Windows 10/11: https://github.com/danielike/hacker-news-api/releases/download/v1.0.1/HackerNewsApi.exe
 - Linux: https://github.com/danielike/hacker-news-api/releases/download/v1.0.1/HackerNewsApi

Windows

Open command prompt and run:
`HackerNewsApi.exe`

Linux

Open terminal and run:
`chmod u+x HackerNewsApi`

`./HackerNewsApi`

By default, the port assigned is 5000. If you want to change it or it is already in use, you can do so running the previous commands following the --urls argument. 

Example:
`HackerNewsApi.exe --urls http://localhost:<PORT>`

**Endpoints**

- `GET /api/v1/news` => Fetch all news ordered by score
- `GET /api/v1/news?amount=n` => Fetch n news ordered by score.

Example: `curl http://localhost:5000/api/v1/news`. Or in your browser, entering the following address: http://localhost:5000/api/v1/news 

**Improvements:**
- Add healthcheck endpoint.
- Better readability and maintainability applying clean architecture, solid principles.
- Add swagger/open api/scalar.
- Add logs and metrics.
- Add tests.
- Better cache handling: shared by multiple instances and surviving to stop services (for example, Redis).
