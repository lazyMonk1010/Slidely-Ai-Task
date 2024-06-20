import { Router, Request, Response } from 'express';
import fs from 'fs';
import path from 'path';

const router = Router();

const dbPath = path.resolve(__dirname, 'db.json');

// Utility function to read the JSON database
const readDatabase = () => {
  const data = fs.readFileSync(dbPath, 'utf-8');
  return JSON.parse(data);
};

// Utility function to write to the JSON database
const writeDatabase = (data: any) => {
  fs.writeFileSync(dbPath, JSON.stringify(data, null, 2));
};

// /ping endpoint
router.get('/ping', (req: Request, res: Response) => {
  res.json(true);
});

// /submit endpoint
router.post('/submit', (req: Request, res: Response) => {
  const { Name, Email, PhoneNumber, GithubLink } = req.body;

  const db = readDatabase();
  db.submissions.push({ Name, Email, PhoneNumber, GithubLink });
  writeDatabase(db);

  res.status(201).json({ message: 'Submission saved successfully' });
});

// /read endpoint
router.get('/read', (req: Request, res: Response) => {
  const index = parseInt(req.query.index as string, 10);

  if (isNaN(index)) {
    return res.status(400).json({ error: 'Index must be a number' });
  }

  const db = readDatabase();
  const submission = db.submissions[index];

  if (!submission) {
    return res.status(404).json({ error: 'Submission not found' });
  }

  res.json(submission);
});

export default router;
