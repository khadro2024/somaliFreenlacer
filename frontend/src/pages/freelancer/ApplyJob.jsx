import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { applicationsApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';
import { FormGroup, Input, Textarea } from '../../components/ui/Input';

export default function ApplyJob() {
  const { jobId } = useParams();
  const [proposal, setProposal] = useState('');
  const [bidAmount, setBidAmount] = useState('');
  const navigate = useNavigate();

  const submit = async (e) => {
    e.preventDefault();
    await applicationsApi.apply(jobId, { proposal, bidAmount: parseFloat(bidAmount) });
    navigate('/freelancer/applications');
  };

  return (
    <div>
      <PageHeader title={`Apply — Job #${jobId}`} />
      <Card className="max-w-xl">
        <form onSubmit={submit}>
          <FormGroup label="Proposal"><Textarea rows={5} value={proposal} onChange={(e) => setProposal(e.target.value)} required /></FormGroup>
          <FormGroup label="Bid Amount ($)"><Input type="number" min="1" value={bidAmount} onChange={(e) => setBidAmount(e.target.value)} required /></FormGroup>
          <Button type="submit">Submit Application</Button>
        </form>
      </Card>
    </div>
  );
}
