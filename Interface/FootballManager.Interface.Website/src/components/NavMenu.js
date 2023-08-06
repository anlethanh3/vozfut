import { Container, Navbar, Nav } from "react-bootstrap";
import { Outlet } from 'react-router-dom'

function NavMenu() {
    return (
        <header>
            <Navbar bg="primary" data-bs-theme="dark">
                <Container>
                    <Navbar.Brand href="/">
                        <img
                            alt=""
                            src="/icon.jpg"
                            width="30"
                            height="30"
                            className="d-inline-block align-top"
                        />{' '}VOZ Football Manager</Navbar.Brand>
                    <Navbar.Toggle />
                    <Navbar.Collapse className="justify-content-end">
                        <Nav>
                            <Nav.Link href="/">Member</Nav.Link>
                            <Nav.Link href="rolling">Rolling</Nav.Link>
                            <Nav.Link href="history">History</Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container>
                <Outlet />
            </Container>
        </header>
    )
}
export default NavMenu;