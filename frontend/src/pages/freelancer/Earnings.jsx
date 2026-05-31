import { useEffect, useState } from 'react';
import { projectsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import PageHeader from '../../components/ui/PageHeader';
import StatCard from '../../components/ui/StatCard';

export default function Earnings() {
  const [projects, setProjects] = useState([]);
  useEffect(() => { projectsApi.getFreelancer().then((r) => setProjects(r.data)).catch(() => {}); }, []);

  const released = projects.filter((p) => p.paymentStatus === 'Released');
  const total = released.reduce((s, p) => s + (p.paymentAmount || 0), 0);

  return (
    <div>
      <PageHeader title="Earnings" />
      <StatCard value={`$${total.toFixed(2)}`} label="Total Released" icon="💰" />
      <div className="mt-6 space-y-3">
        {released.map((p) => (
          <Card key={p.projectId} className="!p-4">
            <p className="text-white">{p.jobTitle}</p>
            <p className="text-sfm-green-400 font-semibold">${p.paymentAmount}</p>
          </Card>
        ))}
      </div>
    </div>
  );
}
