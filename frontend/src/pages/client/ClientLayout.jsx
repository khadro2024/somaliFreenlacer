import { Outlet } from 'react-router-dom';
import DashboardSidebar from '../../components/DashboardSidebar';

const links = [
  { to: '/client', label: 'Overview', end: true },
  { to: '/client/jobs/new', label: 'Create Job' },
  { to: '/client/jobs', label: 'My Jobs' },
  { to: '/client/payments', label: 'Payments' },
  { to: '/client/messages', label: 'Messages' },
];

export default function ClientLayout() {
  return (
    <div className="mx-auto max-w-7xl">
      <div className="grid gap-8 lg:grid-cols-[260px_1fr]">
        <DashboardSidebar title="Client Panel" links={links} />
        <div className="min-w-0"><Outlet /></div>
      </div>
    </div>
  );
}
