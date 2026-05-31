import { useEffect, useState } from 'react';
import { adminApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';

export default function AdminUsers() {
  const [users, setUsers] = useState([]);
  const load = () => adminApi.users().then((r) => setUsers(r.data)).catch(() => {});
  useEffect(() => { load(); }, []);

  return (
    <div>
      <PageHeader title="Users" />
      <Card className="overflow-x-auto p-0">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-sfm-border bg-sfm-surface/80 text-sfm-green-400">
            <tr><th className="px-4 py-3">Name</th><th className="px-4 py-3">Email</th><th className="px-4 py-3">Role</th><th className="px-4 py-3">Verified</th><th className="px-4 py-3">Actions</th></tr>
          </thead>
          <tbody>
            {users.map((u) => (
              <tr key={u.userId} className="border-b border-sfm-border/50 hover:bg-white/[0.02]">
                <td className="px-4 py-3 text-white">{u.fullName}</td>
                <td className="px-4 py-3 text-gray-400">{u.email}</td>
                <td className="px-4 py-3">{u.role}</td>
                <td className="px-4 py-3">{u.isVerified ? '✓' : '—'}</td>
                <td className="px-4 py-3">
                  {!u.isVerified && u.role !== 'Admin' && (
                    <Button variant="outline" size="sm" onClick={() => adminApi.verify(u.userId).then(load)}>Verify</Button>
                  )}
                  {u.role !== 'Admin' && (
                    <Button variant="danger" size="sm" className="ml-2" onClick={() => adminApi.suspend(u.userId, !u.isSuspended).then(load)}>
                      {u.isSuspended ? 'Unsuspend' : 'Suspend'}
                    </Button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </Card>
    </div>
  );
}
