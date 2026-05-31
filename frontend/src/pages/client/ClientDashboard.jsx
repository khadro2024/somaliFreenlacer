import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { jobsApi, projectsApi } from '../../api/client';
import PageHeader from '../../components/ui/PageHeader';
import StatCard from '../../components/ui/StatCard';
import Button from '../../components/ui/Button';

export default function ClientDashboard() {
  const [jobs, setJobs] = useState([]);
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    jobsApi.getMy().then((r) => setJobs(r.data)).catch(() => {});
    projectsApi.getClient().then((r) => setProjects(r.data)).catch(() => {});
  }, []);

  return (
    <div>
      <PageHeader
        title="Client Dashboard"
        subtitle="Maamul shaqooyinkaaga iyo mashaariicda"
        action={<Link to="/client/jobs/new"><Button>+ Post Job</Button></Link>}
      />
      <div className="grid gap-4 sm:grid-cols-3">
        <StatCard value={jobs.length} label="My Jobs" icon="📋" />
        <StatCard value={projects.filter((p) => p.status === 'Working').length} label="Active Projects" icon="🚀" />
        <StatCard value={jobs.filter((j) => j.status === 'Open').length} label="Open Jobs" icon="📂" />
      </div>
    </div>
  );
}
