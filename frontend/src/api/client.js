import axios from 'axios';

const API_BASE = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const client = axios.create({
  baseURL: API_BASE,
  headers: { 'Content-Type': 'application/json' },
});

client.interceptors.request.use((config) => {
  const token = localStorage.getItem('sfm_token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

client.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err.response?.status === 401) {
      localStorage.removeItem('sfm_token');
      localStorage.removeItem('sfm_user');
      if (!window.location.pathname.includes('/login')) {
        window.location.href = '/login';
      }
    }
    return Promise.reject(err);
  }
);

export const authApi = {
  register: (data) => client.post('/auth/register', data),
  login: (data) => client.post('/auth/login', data),
};

export const jobsApi = {
  getAll: (params) => client.get('/jobs', { params }),
  getById: (id) => client.get(`/jobs/${id}`),
  getMy: () => client.get('/jobs/my'),
  create: (data) => client.post('/jobs', data),
  update: (id, data) => client.put(`/jobs/${id}`, data),
  delete: (id) => client.delete(`/jobs/${id}`),
};

export const applicationsApi = {
  apply: (jobId, data) => client.post(`/applications/jobs/${jobId}`, data),
  getMy: () => client.get('/applications/my'),
  getByJob: (jobId) => client.get(`/applications/jobs/${jobId}`),
  accept: (id) => client.post(`/applications/${id}/accept`),
  reject: (id) => client.post(`/applications/${id}/reject`),
};

export const projectsApi = {
  getClient: () => client.get('/projects/client'),
  getFreelancer: () => client.get('/projects/freelancer'),
  getById: (id) => client.get(`/projects/${id}`),
  clientComplete: (id) => client.post(`/projects/${id}/complete`),
  freelancerSubmit: (id) => client.post(`/projects/${id}/submit`),
};

export const paymentsApi = {
  fund: (projectId, data) => client.post(`/payments/projects/${projectId}/fund`, data),
  release: (projectId) => client.post(`/payments/projects/${projectId}/release`),
  getByProject: (projectId) => client.get(`/payments/projects/${projectId}`),
  getAll: () => client.get('/payments'),
};

export const messagesApi = {
  send: (data) => client.post('/messages', data),
  inbox: () => client.get('/messages/inbox'),
  conversation: (userId) => client.get(`/messages/conversation/${userId}`),
};

export const profilesApi = {
  getFreelancers: () => client.get('/profiles/freelancers'),
  getProfile: (userId) => client.get(`/profiles/${userId}`),
  updateMe: (data) => client.put('/profiles/me', data),
  rate: (projectId, data) => client.post(`/profiles/projects/${projectId}/rate`, data),
};

export const categoriesApi = {
  getAll: () => client.get('/categories'),
};

export const mediaApi = {
  getConfig: () => client.get('/media/config'),
  upload: (formData) => client.post('/media/upload', formData, { headers: { 'Content-Type': 'multipart/form-data' } }),
  getPortfolio: (userId) => client.get(`/media/portfolio/${userId}`),
  getProjectImages: (projectId) => client.get(`/media/project/${projectId}`),
  delete: (id) => client.delete(`/media/${id}`),
};

export const demoApi = {
  getAccounts: () => client.get('/demo/accounts'),
};

export const adminApi = {
  stats: () => client.get('/admin/stats'),
  users: () => client.get('/admin/users'),
  jobs: () => client.get('/admin/jobs'),
  suspend: (id, suspend) => client.post(`/admin/users/${id}/suspend?suspend=${suspend}`),
  verify: (id) => client.post(`/admin/users/${id}/verify`),
};

export default client;
