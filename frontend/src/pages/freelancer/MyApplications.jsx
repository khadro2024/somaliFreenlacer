import { useEffect, useState } from 'react';
import { applicationsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Badge from '../../components/ui/Badge';
import PageHeader from '../../components/ui/PageHeader';

export default function MyApplications() {
  const [apps, setApps] = useState([]);
  useEffect(() => { applicationsApi.getMy().then((r) => setApps(r.data)).catch(() => {}); }, []);

  return (
    <div>
      <PageHeader title="My Applications" />
      <div className="space-y-4">
        {apps.map((a) => (
          <Card key={a.applicationId}>
            <div className="flex justify-between gap-4">
              <h3 className="font-semibold text-white">{a.jobTitle}</h3>
              <Badge status={a.status} />
            </div>
            <p className="mt-2 text-sfm-green-400">${a.bidAmount}</p>
            <p className="mt-2 text-sm text-gray-400">{a.proposal}</p>
          </Card>
        ))}
      </div>
    </div>
  );
}
