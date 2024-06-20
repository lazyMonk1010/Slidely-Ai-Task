# Express TypeScript Backend

## Setup

1. Clone the repository.
2. Run `npm install` to install dependencies.
3. Run `npm run build` to compile TypeScript to JavaScript.
4. Run `npm start` to start the server.

## API Endpoints

### /ping
- Method: GET
- Description: Returns `true` to check if the server is running.

### /submit
- Method: POST
- Description: Saves a form submission.
- Body Parameters:
  - `name` (string)
  - `email` (string)
  - `phone` (string)
  - `github_link` (string)
  - `stopwatch_time` (string)

### /read
- Method: GET
- Description: Retrieves a form submission by index.
- Query Parameters:
  - `index` (number) - The index of the submission to retrieve.
