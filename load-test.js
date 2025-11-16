// load-test.js — 100% SLI: RPS=5, p95<300ms, 99.9% успех
import http from 'k6/http';
import { check, sleep } from 'k6';
import { htmlReport } from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';
import { textSummary } from 'https://jslib.k6.io/k6-summary/0.0.1/index.js';

const BASE_URL = 'http://localhost:8080';
const RPS = 5;
const DURATION = '5m'; 

export const options = {
  stages: [
    { duration: '10s', target: RPS },
    { duration: DURATION, target: RPS },
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<300'],
    'http_req_duration{scenario:deactivate}': ['p(95)<100'],
    http_req_failed: ['rate<0.001'],  // 99.9%
    checks: ['rate>0.999'],
  },
};

let teamName; 

export function setup() {
  teamName = `loadteam_${Date.now()}`;
  const payload = {
    team_name: teamName,
    members: Array.from({ length: 10 }, (_, i) => ({
      user_id: `u${i + 1}`,
      username: `User ${i + 1}`,
      is_active: true,
    })),
  };

  const res = http.post(`${BASE_URL}/team/add`, JSON.stringify(payload), {
    headers: { 'Content-Type': 'application/json' },
  });

  check(res, { 'team created': (r) => r.status === 201 });
  return { teamName };
}

export default function (data) {
  const { teamName } = data;
  const prId = `pr-${__VU}-${Date.now()}`;
  const authorId = 'u1';

  // Создание pr
  const createRes = http.post(`${BASE_URL}/pullRequest/create`, JSON.stringify({
    pull_request_id: prId,
    pull_request_name: 'Load Test PR',
    author_id: authorId,
    team_name: teamName, // Явно указываем команду
  }), {
    headers: { 'Content-Type': 'application/json' },
  });

  const createOk = check(createRes, {
    'PR created': (r) => r.status === 201,
  });

  if (!createOk) {
    sleep(1);
    return;
  }

  // Деактивация
  const deactivateRes = http.post(`${BASE_URL}/team/deactivateUsers`, JSON.stringify({
    UserIds: ['u2', 'u3', 'u4'],
    ReassignOpenPRs: true,
  }), {
    headers: { 'Content-Type': 'application/json' },
    tags: { scenario: 'deactivate' },
  });

  check(deactivateRes, {
    'deactivate ok': (r) => r.status === 200,
    'deactivate < 100ms': (r) => r.timings.duration < 100,
  });

  sleep(1);
}

export function handleSummary(data) {
  return {
    'k6-report.html': htmlReport(data),
    stdout: textSummary(data, { indent: ' ', enableColors: true }),
  };
}