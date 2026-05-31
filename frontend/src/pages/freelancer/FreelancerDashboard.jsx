import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { applicationsApi, projectsApi } from '../../api/client';
import PageHeader from '../../components/ui/PageHeader';
import StatCard from '../../components/ui/StatCard';
import Button from '../../components/ui/Button';

export default function FreelancerDashboard() {
  const [apps, setApps] = useState([]);
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    applicationsApi.getMy().then((r) => setApps(r.data)).catch(() => {});
    projectsApi.getFreelancer().then((r) => setProjects(r.data)).catch(() => {});
  }, []);

  return (
    <div>
      <PageHeader title="Freelancer Dashboard" subtitle="Raadiso fursadaha shaqada" action={<Link to="/freelancer/jobs"><Button>Browse Jobs</Button></Link>} />
      <div className="grid gap-4 sm:grid-cols-2">
        <StatCard value={apps.length} label="Applications" icon="📝" />
        <StatCard value={projects.length} label="Projects" icon="💼" />
      </div>
    </div>
  );
}
