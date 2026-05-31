import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { demoApi } from '../api/client';
import Card from '../components/ui/Card';
import Button from '../components/ui/Button';
import Alert from '../components/ui/Alert';
import { FormGroup, Input } from '../components/ui/Input';
import PageHeader from '../components/ui/PageHeader';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [demoAccounts, setDemoAccounts] = useState([]);
  const { login, loading } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    demoApi.getAccounts().then((r) => setDemoAccounts(r.data)).catch(() => {});
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const user = await login(email, password);
      if (user.role === 'Admin') navigate('/admin');
      else if (user.role === 'Client') navigate('/client');
      else navigate('/freelancer');
    } catch {
      setError('Email ama password khaldan.');
    }
  };

  const fillDemo = (acc) => {
    setEmail(acc.email);
    setPassword(acc.password);
  };

  return (
    <div className="mx-auto max-w-4xl px-4 py-8">
      <div className="grid gap-8 lg:grid-cols-2">
        <Card className="w-full">
          <PageHeader title="Login" subtitle="Ku soo gal account-kaaga" />
          {error && <Alert>{error}</Alert>}
          <form onSubmit={handleSubmit} className="space-y-1">
            <FormGroup label="Email">
              <Input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
            </FormGroup>
            <FormGroup label="Password">
              <Input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
            </FormGroup>
            <Button type="submit" className="mt-4 w-full" disabled={loading}>
              {loading ? 'Loading...' : 'Login'}
            </Button>
          </form>
          <p className="mt-6 text-center text-sm text-gray-500">
            Ma haysatid account?{' '}
            <Link to="/register" className="font-medium text-sfm-green-400 hover:text-sfm-green-300">Register</Link>
          </p>
        </Card>

        <Card className="w-full">
          <h2 className="mb-2 text-lg font-semibold text-sfm-green-400">Xogta tusaale (Demo)</h2>
          <p className="mb-4 text-sm text-gray-500">Guji account si email/password loo buuxiyo</p>
          <div className="max-h-[420px] space-y-2 overflow-y-auto">
            {demoAccounts.map((acc) => (
              <button
                key={acc.email}
                type="button"
                onClick={() => fillDemo(acc)}
                className="w-full rounded-xl border border-sfm-border/80 bg-sfm-surface/50 px-4 py-3 text-left transition hover:border-sfm-green-700 hover:bg-sfm-green-950/30"
              >
                <div className="flex items-center justify-between gap-2">
                  <span className="font-medium text-white">{acc.fullName}</span>
                  <span className="shrink-0 rounded-full bg-sfm-green-950 px-2 py-0.5 text-xs text-sfm-green-400">{acc.role}</span>
                </div>
                <p className="mt-1 text-xs text-gray-500">{acc.email} · {acc.password}</p>
                <p className="mt-1 text-xs text-gray-400">{acc.purpose}</p>
              </button>
            ))}
          </div>
        </Card>
      </div>
    </div>
  );
}
