import { Outlet } from 'react-router-dom';
import DashboardSidebar from '../../components/DashboardSidebar';

const links = [
  { to: '/admin', label: 'Dashboard', end: true },
  { to: '/admin/users', label: 'Users' },
  { to: '/admin/jobs', label: 'Jobs' },
  { to: '/admin/payments', label: 'Payments' },
];

export default function AdminLayout() {
  return (
    <div className="mx-auto max-w-7xl">
      <div className="grid gap-8 lg:grid-cols-[260px_1fr]">
        <DashboardSidebar title="Admin Panel" links={links} />
        <div className="min-w-0"><Outlet /></div>
      </div>
    </div>
  );
}
