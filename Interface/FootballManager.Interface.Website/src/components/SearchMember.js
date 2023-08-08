import { useState } from "react"
import { Button, Accordion, Form, FormGroup, FloatingLabel } from "react-bootstrap";

const SearchMember = ({ onSearchChanged: onSearchChanged, onSubmit }) => {

    const [name, setName] = useState('')

    const onChanged = (e) => {
        const value = e.target.value
        onSearchChanged({ name: name })
        setName(value)
    }

    const onSearch = () => {
        onSubmit({ name: name })
    }

    const onClear = () => {
        setName('')
    }

    return (
        <Accordion className="my-2" defaultActiveKey="0">
            <Accordion.Item eventKey="0">
                <Accordion.Header>Search box</Accordion.Header>
                <Accordion.Body>
                    <Form>
                        <FormGroup className="mb-3" controlId="addMember.Name">
                            <FloatingLabel controlId="floatingName" label="Name">
                                <Form.Control type="text" placeholder="Input name" value={name} onChange={onChanged} />
                            </FloatingLabel>
                        </FormGroup>
                    </Form>
                    <Button className="me-2" variant="primary" onClick={onSearch}>
                        Search
                    </Button>
                    <Button variant="secondary" onClick={onClear}>
                        Clear
                    </Button>
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    )
}

export default SearchMember