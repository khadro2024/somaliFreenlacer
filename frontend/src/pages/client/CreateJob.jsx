import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { jobsApi, categoriesApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Button from '../../components/ui/Button';
import Alert from '../../components/ui/Alert';
import PageHeader from '../../components/ui/PageHeader';
import { FormGroup, Input, Textarea, Select } from '../../components/ui/Input';
import ImageUpload from '../../components/ui/ImageUpload';

export default function CreateJob() {
  const [categories, setCategories] = useState([]);
  const [form, setForm] = useState({ title: '', description: '', budget: '', categoryId: '' });
  const [error, setError] = useState('');
  const [createdJobId, setCreatedJobId] = useState(null);
  const [coverUrl, setCoverUrl] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    categoriesApi.getAll().then((r) => setCategories(r.data)).catch(() => {});
  }, []);

  const submit = async (e) => {
    e.preventDefault();
    try {
      const { data } = await jobsApi.create({
        title: form.title,
        description: form.description,
        budget: parseFloat(form.budget),
        categoryId: form.categoryId ? parseInt(form.categoryId, 10) : null,
      });
      setCreatedJobId(data.jobId);
      if (!coverUrl) navigate('/client/jobs');
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to create job');
    }
  };

  return (
    <div>
      <PageHeader title="Create Job" subtitle="Daabac shaqo cusub" />
      {error && <Alert>{error}</Alert>}
      <Card className="max-w-xl">
        <form onSubmit={submit}>
          <FormGroup label="Title"><Input value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} required /></FormGroup>
          <FormGroup label="Description"><Textarea rows={5} value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} required /></FormGroup>
          <FormGroup label="Budget ($)"><Input type="number" min="1" value={form.budget} onChange={(e) => setForm({ ...form, budget: e.target.value })} required /></FormGroup>
          <FormGroup label="Category">
            <Select value={form.categoryId} onChange={(e) => setForm({ ...form, categoryId: e.target.value })}>
              <option value="">Select category</option>
              {categories.map((c) => <option key={c.categoryId} value={c.categoryId}>{c.name}</option>)}
            </Select>
          </FormGroup>
          {!createdJobId ? (
            <Button type="submit">Post Job</Button>
          ) : (
            <>
              <Alert variant="success">Job created! Hadda soo geli sawirka cover-ka.</Alert>
              {coverUrl && <img src={coverUrl} alt="Cover" className="my-4 h-40 w-full rounded-xl object-cover" />}
              <ImageUpload type="jobcover" jobId={createdJobId} label="Sawirka job-ka (cover)" onUploaded={(d) => { setCoverUrl(d.imageUrl); setTimeout(() => navigate('/client/jobs'), 800); }} />
              <Button type="button" variant="outline" className="mt-4" onClick={() => navigate('/client/jobs')}>Skip — Jobs list</Button>
            </>
          )}
        </form>
      </Card>
    </div>
  );
}
