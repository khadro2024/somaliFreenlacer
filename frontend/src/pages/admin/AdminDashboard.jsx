import { useEffect, useState } from 'react';
import { adminApi } from '../../api/client';
import PageHeader from '../../components/ui/PageHeader';
import StatCard from '../../components/ui/StatCard';

export default function AdminDashboard() {
  const [stats, setStats] = useState(null);
  useEffect(() => { adminApi.stats().then((r) => setStats(r.data)).catch(() => {}); }, []);
  if (!stats) return <p className="text-gray-500">Loading...</p>;

  return (
    <div>
      <PageHeader title="Admin Dashboard" subtitle="System overview" />
      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        <StatCard value={stats.totalUsers} label="Users" icon="👥" />
        <StatCard value={stats.totalJobs} label="Jobs" icon="📋" />
        <StatCard value={stats.activeProjects} label="Active Projects" icon="🚀" />
        <StatCard value={stats.pendingPayments} label="Held Payments" icon="🔒" />
      </div>
    </div>
  );
}
