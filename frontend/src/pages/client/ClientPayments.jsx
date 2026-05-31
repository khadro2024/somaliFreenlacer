import { useEffect, useState } from 'react';
import { projectsApi, paymentsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Badge from '../../components/ui/Badge';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';

export default function ClientPayments() {
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    projectsApi.getClient().then((r) => setProjects(r.data)).catch(() => {});
  }, []);

  const fund = async (projectId, amount) => {
    await paymentsApi.fund(projectId, { amount: parseFloat(amount) });
    const r = await projectsApi.getClient();
    setProjects(r.data);
  };

  const release = async (projectId) => {
    try {
      await paymentsApi.release(projectId);
      const r = await projectsApi.getClient();
      setProjects(r.data);
    } catch (err) {
      alert(err.response?.data?.message || 'Release failed');
    }
  };

  return (
    <div>
      <PageHeader title="Payments" subtitle="Escrow — lacag ammaan ah" />
      <div className="space-y-4">
        {projects.map((p) => (
          <Card key={p.projectId}>
            <div className="flex flex-wrap items-center justify-between gap-4">
              <div>
                <h3 className="font-semibold text-white">{p.jobTitle}</h3>
                <p className="mt-1 text-sm text-gray-500">Project · {p.status}</p>
              </div>
              <Badge status={p.paymentStatus || 'Pending'} />
            </div>
            <p className="mt-3 text-xl font-bold text-sfm-green-400">${p.paymentAmount ?? 0}</p>
            <div className="mt-4 flex gap-2">
              {p.paymentStatus === 'Pending' && (
                <Button size="sm" onClick={() => fund(p.projectId, p.paymentAmount || 100)}>Fund Escrow</Button>
              )}
              {p.paymentStatus === 'Held' && p.status === 'Completed' && (
                <Button size="sm" onClick={() => release(p.projectId)}>Release Payment</Button>
              )}
            </div>
          </Card>
        ))}
      </div>
    </div>
  );
}
