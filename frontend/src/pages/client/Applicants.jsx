import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { applicationsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Badge from '../../components/ui/Badge';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';

export default function Applicants() {
  const { jobId } = useParams();
  const [apps, setApps] = useState([]);
  const load = () => applicationsApi.getByJob(jobId).then((r) => setApps(r.data)).catch(() => {});
  useEffect(() => { load(); }, [jobId]);

  return (
    <div>
      <PageHeader title="Applicants" subtitle={`Job #${jobId}`} />
      <div className="space-y-4">
        {apps.map((a) => (
          <Card key={a.applicationId}>
            <div className="flex flex-wrap items-start justify-between gap-4">
              <div>
                <h3 className="text-lg font-semibold text-white">{a.freelancerName}</h3>
                <p className="mt-1 text-sm text-sfm-green-400">★ {a.freelancerRating?.toFixed(1) ?? 'N/A'} · Bid: ${a.bidAmount}</p>
              </div>
              <Badge status={a.status} />
            </div>
            <p className="mt-4 text-gray-400">{a.proposal}</p>
            {a.status === 'Pending' && (
              <div className="mt-4 flex gap-2">
                <Button size="sm" onClick={() => accept(a.applicationId)}>Accept</Button>
                <Button variant="danger" size="sm" onClick={() => reject(a.applicationId)}>Reject</Button>
              </div>
            )}
          </Card>
        ))}
      </div>
    </div>
  );

  async function accept(id) { await applicationsApi.accept(id); load(); }
  async function reject(id) { await applicationsApi.reject(id); load(); }
}
