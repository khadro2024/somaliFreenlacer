import { useEffect, useState } from 'react';
import { adminApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Badge from '../../components/ui/Badge';
import PageHeader from '../../components/ui/PageHeader';

export default function AdminJobs() {
  const [jobs, setJobs] = useState([]);
  useEffect(() => { adminApi.jobs().then((r) => setJobs(r.data)).catch(() => {}); }, []);

  return (
    <div>
      <PageHeader title="All Jobs" />
      <div className="grid gap-4 sm:grid-cols-2">
        {jobs.map((j) => (
          <Card key={j.jobId}>
            <Badge status={j.status} />
            <h3 className="mt-2 font-semibold text-white">{j.title}</h3>
            <p className="mt-1 text-sm text-gray-500">{j.clientName} · ${j.budget}</p>
          </Card>
        ))}
      </div>
    </div>
  );
}
