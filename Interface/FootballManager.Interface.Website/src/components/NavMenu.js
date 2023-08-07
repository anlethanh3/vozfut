import { Container, Navbar, Nav, Button } from "react-bootstrap";
import { Outlet } from 'react-router-dom'

function NavMenu() {
    return (
        <header>
            <Navbar expand="md" bg="primary" data-bs-theme="dark">
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
                        <Nav className="me-auto">
                            <Nav.Link href="/">Member</Nav.Link>
                            <Nav.Link href="rolling">Rolling</Nav.Link>
                            <Nav.Link href="history">History</Nav.Link>
                        </Nav>
                        <Nav>
                            <Button>Login</Button>
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