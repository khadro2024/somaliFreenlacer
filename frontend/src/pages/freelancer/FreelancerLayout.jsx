import { Outlet } from 'react-router-dom';
import DashboardSidebar from '../../components/DashboardSidebar';

const links = [
  { to: '/freelancer', label: 'Overview', end: true },
  { to: '/freelancer/profile', label: 'Profile' },
  { to: '/freelancer/jobs', label: 'Available Jobs' },
  { to: '/freelancer/applications', label: 'My Applications' },
  { to: '/freelancer/projects', label: 'Active Projects' },
  { to: '/freelancer/earnings', label: 'Earnings' },
  { to: '/freelancer/messages', label: 'Messages' },
];

export default function FreelancerLayout() {
  return (
    <div className="mx-auto max-w-7xl">
      <div className="grid gap-8 lg:grid-cols-[260px_1fr]">
        <DashboardSidebar title="Freelancer Panel" links={links} />
        <div className="min-w-0"><Outlet /></div>
      </div>
    </div>
  );
}
