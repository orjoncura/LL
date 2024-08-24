
import Navbar from '@/components/Navbar/Navbar/Navbar';

export default function Page({ params }: { params: { id: string } }) {
  return (
    <div>      
      <Navbar /> 
      My Post: {params.id}
    </div>
    );
}