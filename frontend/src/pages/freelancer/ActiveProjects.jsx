import { useEffect, useState } from 'react';
import { projectsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Badge from '../../components/ui/Badge';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';
import ImageUpload, { ImageGallery } from '../../components/ui/ImageUpload';

export default function ActiveProjects() {
  const [projects, setProjects] = useState([]);

  const load = () => projectsApi.getFreelancer().then((r) => setProjects(r.data)).catch(() => {});
  useEffect(() => { load(); }, []);

  const submit = async (id) => {
    await projectsApi.freelancerSubmit(id);
    load();
  };

  return (
    <div>
      <PageHeader title="Active Projects" subtitle="Soo geli sawirrada shaqada aad qabatay" />
      <div className="space-y-6">
        {projects.map((p) => (
          <Card key={p.projectId}>
            <div className="flex flex-wrap justify-between gap-2">
              <h3 className="font-semibold text-white">{p.jobTitle}</h3>
              <Badge status={p.status} />
            </div>
            <p className="mt-2 text-sm text-gray-500">Client: {p.clientName} · Payment: {p.paymentStatus}</p>
            <ImageGallery images={p.workImages} />
            <ImageUpload
              type="projectwork"
              projectId={p.projectId}
              label="Ku dar sawir shaqo (deliverable)"
              onUploaded={load}
              className="mt-4"
            />
            {p.status === 'Working' && (
              <Button size="sm" className="mt-4" onClick={() => submit(p.projectId)}>Mark Work Complete</Button>
            )}
          </Card>
        ))}
      </div>
    </div>
  );
}
