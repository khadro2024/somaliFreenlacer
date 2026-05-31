import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import Home from './pages/Home';
import About from './pages/About';
import BrowseJobs from './pages/BrowseJobs';
import FreelancersList from './pages/FreelancersList';
import Login from './pages/Login';
import Register from './pages/Register';
import ClientLayout from './pages/client/ClientLayout';
import ClientDashboard from './pages/client/ClientDashboard';
import CreateJob from './pages/client/CreateJob';
import MyJobs from './pages/client/MyJobs';
import Applicants from './pages/client/Applicants';
import ClientPayments from './pages/client/ClientPayments';
import ClientMessages from './pages/client/ClientMessages';
import FreelancerLayout from './pages/freelancer/FreelancerLayout';
import FreelancerDashboard from './pages/freelancer/FreelancerDashboard';
import Profile from './pages/freelancer/Profile';
import AvailableJobs from './pages/freelancer/AvailableJobs';
import ApplyJob from './pages/freelancer/ApplyJob';
import MyApplications from './pages/freelancer/MyApplications';
import ActiveProjects from './pages/freelancer/ActiveProjects';
import Earnings from './pages/freelancer/Earnings';
import FreelancerMessages from './pages/freelancer/FreelancerMessages';
import AdminLayout from './pages/admin/AdminLayout';
import AdminDashboard from './pages/admin/AdminDashboard';
import AdminUsers from './pages/admin/AdminUsers';
import AdminJobs from './pages/admin/AdminJobs';
import AdminPayments from './pages/admin/AdminPayments';

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<Layout />}>
            <Route index element={<Home />} />
            <Route path="about" element={<About />} />
            <Route path="jobs" element={<BrowseJobs />} />
            <Route path="freelancers" element={<FreelancersList />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
            <Route path="client" element={<ProtectedRoute roles={['Client']}><ClientLayout /></ProtectedRoute>}>
              <Route index element={<ClientDashboard />} />
              <Route path="jobs/new" element={<CreateJob />} />
              <Route path="jobs" element={<MyJobs />} />
              <Route path="jobs/:jobId/applicants" element={<Applicants />} />
              <Route path="payments" element={<ClientPayments />} />
              <Route path="messages" element={<ClientMessages />} />
            </Route>
            <Route path="freelancer" element={<ProtectedRoute roles={['Freelancer']}><FreelancerLayout /></ProtectedRoute>}>
              <Route index element={<FreelancerDashboard />} />
              <Route path="profile" element={<Profile />} />
              <Route path="jobs" element={<AvailableJobs />} />
              <Route path="apply/:jobId" element={<ApplyJob />} />
              <Route path="applications" element={<MyApplications />} />
              <Route path="projects" element={<ActiveProjects />} />
              <Route path="earnings" element={<Earnings />} />
              <Route path="messages" element={<FreelancerMessages />} />
            </Route>
            <Route path="admin" element={<ProtectedRoute roles={['Admin']}><AdminLayout /></ProtectedRoute>}>
              <Route index element={<AdminDashboard />} />
              <Route path="users" element={<AdminUsers />} />
              <Route path="jobs" element={<AdminJobs />} />
              <Route path="payments" element={<AdminPayments />} />
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
