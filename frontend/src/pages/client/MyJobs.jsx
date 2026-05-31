import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { jobsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Badge from '../../components/ui/Badge';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';

export default function MyJobs() {
  const [jobs, setJobs] = useState([]);

  useEffect(() => {
    jobsApi.getMy().then((r) => setJobs(r.data)).catch(() => {});
  }, []);

  return (
    <div>
      <PageHeader title="My Jobs" action={<Link to="/client/jobs/new"><Button size="sm">+ New Job</Button></Link>} />
      <div className="grid gap-4 sm:grid-cols-2">
        {jobs.map((job) => (
          <Card key={job.jobId} hover>
            <Badge status={job.status} />
            <h3 className="mt-3 text-lg font-semibold text-white">{job.title}</h3>
            <p className="mt-2 text-sfm-green-400 font-semibold">${job.budget}</p>
            <p className="text-sm text-gray-500">{job.applicationCount} applicants</p>
            <Link to={`/client/jobs/${job.jobId}/applicants`} className="mt-4 inline-block">
              <Button variant="outline" size="sm">View Applicants</Button>
            </Link>
          </Card>
        ))}
      </div>
    </div>
  );
}
