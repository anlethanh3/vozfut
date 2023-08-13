import './App.css';
import { RouterProvider, createBrowserRouter, createRoutesFromElements, Route } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import Layout from './components/Layout';
import History from './pages/History';
import Match from './pages/Match';
import ErrorPage from './pages/ErrorPage';
import Member from './pages/Member';
import Donate from './pages/Donate';
import { Counter } from './features/counter/counter';
import Home from './pages/Home';
function App() {
  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="match" element={<Match />} />
        <Route path="donate" element={<Donate />} />
        <Route path="member" element={<Member />} />
        <Route path="history" element={<History />} />
        <Route path="counter" element={<Counter />} />
        <Route path='*' element={<ErrorPage />} />
      </Route>
    )
  )
  return (
    <RouterProvider router={router} />
  );
}

export default App;
