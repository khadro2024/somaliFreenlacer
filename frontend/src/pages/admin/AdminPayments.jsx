import { useEffect, useState } from 'react';
import { paymentsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import PageHeader from '../../components/ui/PageHeader';

export default function AdminPayments() {
  const [payments, setPayments] = useState([]);
  useEffect(() => { paymentsApi.getAll().then((r) => setPayments(r.data)).catch(() => {}); }, []);

  return (
    <div>
      <PageHeader title="Payments Monitor" />
      <Card className="overflow-x-auto p-0">
        <table className="w-full text-left text-sm">
          <thead className="border-b border-sfm-border bg-sfm-surface/80 text-sfm-green-400">
            <tr><th className="px-4 py-3">ID</th><th className="px-4 py-3">Project</th><th className="px-4 py-3">Amount</th><th className="px-4 py-3">Status</th></tr>
          </thead>
          <tbody>
            {payments.map((p) => (
              <tr key={p.paymentId} className="border-b border-sfm-border/50">
                <td className="px-4 py-3">{p.paymentId}</td>
                <td className="px-4 py-3 text-gray-400">{p.projectId}</td>
                <td className="px-4 py-3 text-sfm-green-400">${p.amount}</td>
                <td className="px-4 py-3">{p.status}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </Card>
    </div>
  );
}
