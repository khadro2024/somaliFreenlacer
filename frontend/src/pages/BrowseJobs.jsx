import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { jobsApi, categoriesApi } from '../api/client';
import { useAuth } from '../context/AuthContext';
import Card from '../components/ui/Card';
import Badge from '../components/ui/Badge';
import Button from '../components/ui/Button';
import PageHeader from '../components/ui/PageHeader';
import { Select } from '../components/ui/Input';

export default function BrowseJobs() {
  const [jobs, setJobs] = useState([]);
  const [categories, setCategories] = useState([]);
  const [status, setStatus] = useState('Open');
  const [categoryId, setCategoryId] = useState('');
  const { user } = useAuth();

  useEffect(() => {
    categoriesApi.getAll().then((r) => setCategories(r.data)).catch(() => {});
  }, []);

  useEffect(() => {
    const params = {};
    if (status) params.status = status;
    if (categoryId) params.categoryId = categoryId;
    jobsApi.getAll(params).then((r) => setJobs(r.data)).catch(() => setJobs([]));
  }, [status, categoryId]);

  return (
    <div className="mx-auto max-w-7xl">
      <PageHeader title="Browse Jobs" subtitle="Hel shaqo ku habboon xirfadaada" />

      <div className="mb-8 flex flex-wrap gap-4">
        <Select value={status} onChange={(e) => setStatus(e.target.value)} className="w-auto min-w-[160px]">
          <option value="">All Status</option>
          <option value="Open">Open</option>
          <option value="InProgress">In Progress</option>
          <option value="Closed">Closed</option>
        </Select>
        <Select value={categoryId} onChange={(e) => setCategoryId(e.target.value)} className="w-auto min-w-[180px]">
          <option value="">All Categories</option>
          {categories.map((c) => (
            <option key={c.categoryId} value={c.categoryId}>{c.name}</option>
          ))}
        </Select>
      </div>

      {jobs.length === 0 ? (
        <p className="py-20 text-center text-gray-500">Shaqo ma jirto hadda.</p>
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {jobs.map((job) => (
            <Card key={job.jobId} hover className="flex flex-col overflow-hidden p-0">
              {job.coverImageUrl && (
                <img src={job.coverImageUrl} alt="" className="h-36 w-full object-cover" />
              )}
              <div className="flex flex-1 flex-col p-6">
              <div className="mb-3 flex items-start justify-between gap-2">
                <Badge status={job.status} />
                <span className="text-lg font-bold text-sfm-green-400">${job.budget}</span>
              </div>
              <h3 className="text-lg font-semibold text-white">{job.title}</h3>
              <p className="mt-2 flex-1 text-sm text-gray-400 line-clamp-3">{job.description}</p>
              <p className="mt-4 text-xs text-gray-500">
                {job.categoryName || 'General'} · {job.applicationCount} applications
              </p>
              {user?.role === 'Freelancer' && job.status === 'Open' && (
                <Link to={`/freelancer/apply/${job.jobId}`} className="mt-4">
                  <Button className="w-full">Apply Now</Button>
                </Link>
              )}
              </div>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
