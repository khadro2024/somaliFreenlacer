import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Card from '../components/ui/Card';
import Button from '../components/ui/Button';
import Alert from '../components/ui/Alert';
import { FormGroup, Input, Select } from '../components/ui/Input';
import PageHeader from '../components/ui/PageHeader';

export default function Register() {
  const [form, setForm] = useState({ fullName: '', email: '', password: '', role: 'client' });
  const [error, setError] = useState('');
  const { register, loading } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const user = await register(form);
      navigate(user.role === 'Client' ? '/client' : '/freelancer');
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed.');
    }
  };

  return (
    <div className="mx-auto flex min-h-[70vh] max-w-md items-center justify-center">
      <Card className="w-full">
        <PageHeader title="Register" subtitle="Abuur account cusub" />
        {error && <Alert>{error}</Alert>}
        <form onSubmit={handleSubmit}>
          <FormGroup label="Full Name">
            <Input value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} required />
          </FormGroup>
          <FormGroup label="Email">
            <Input type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} required />
          </FormGroup>
          <FormGroup label="Password">
            <Input type="password" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} required minLength={6} />
          </FormGroup>
          <FormGroup label="Role">
            <Select value={form.role} onChange={(e) => setForm({ ...form, role: e.target.value })}>
              <option value="client">Client (Shaqo bixiye)</option>
              <option value="freelancer">Freelancer (Shaqo qabe)</option>
            </Select>
          </FormGroup>
          <Button type="submit" className="mt-2 w-full" disabled={loading}>
            {loading ? 'Loading...' : 'Register'}
          </Button>
        </form>
        <p className="mt-6 text-center text-sm text-gray-500">
          Horey u haysatay?{' '}
          <Link to="/login" className="font-medium text-sfm-green-400 hover:text-sfm-green-300">Login</Link>
        </p>
      </Card>
    </div>
  );
}
